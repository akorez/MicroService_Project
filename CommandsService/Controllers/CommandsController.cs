using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
	[Route("api/c/platforms/{platformId}/[controller]")]
	[ApiController]
	public class CommandsController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly ICommandRepo _commandRepo;

		public CommandsController(IMapper mapper, ICommandRepo commandRepo)
		{
			_mapper = mapper;
			_commandRepo = commandRepo;
		}

		[HttpGet]
		public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
		{
			Console.WriteLine($"--> Hit GetCommandsForPlatform: {platformId}");

			if (!_commandRepo.PlatformExist(platformId))
			{
				return NotFound();
			}

			var commands = _commandRepo.GetCommandsForPlatform(platformId);
			var result = _mapper.Map<IEnumerable<CommandReadDto>>(commands);

			return Ok(result);
		}

		[HttpGet("{commandId}", Name = "GetCommandForPlatform")]
		public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId)
		{
			Console.WriteLine($"--> Hit GetCommandForPlatform: {platformId} / {commandId}");

			if (!_commandRepo.PlatformExist(platformId))
			{
				return NotFound();
			}

			var command = _commandRepo.GetCommand(platformId, commandId);

			if (command == null)
			{
				return NotFound();
			}

			return Ok(_mapper.Map<CommandReadDto>(command));
		}

		[HttpPost]
		public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, CommandCreateDto commandDto)
		{
			Console.WriteLine($"--> Hit CreateCommandForPlatform: {platformId}");

			if (!_commandRepo.PlatformExist(platformId))
			{
				return NotFound();
			}

			var command = _mapper.Map<Command>(commandDto);

			_commandRepo.CreateCommand(platformId, command);
			_commandRepo.SaveChanges();

			var commandReadDto = _mapper.Map<CommandReadDto>(command);

			return CreatedAtRoute(nameof(GetCommandForPlatform),
				new { platformId = platformId, commandId = commandReadDto.Id }, commandReadDto);
		}
	}
}
