using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PlatformsController : ControllerBase
	{
		private readonly IPlatformRepository _platformRepository;
		private readonly IMapper _mapper;
		private readonly ICommandDataClient _commandDataClient;

		public PlatformsController(IPlatformRepository platformRepository, IMapper mapper, ICommandDataClient commandDataClient)
		{
			_platformRepository = platformRepository;
			_mapper = mapper;
			_commandDataClient = commandDataClient;
		}

		[HttpGet]
		public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms() 
		{
			Console.WriteLine("-->Getting Platforms");

			var platformItemsList = _platformRepository.GetAllPlatforms();

			if (platformItemsList == null)
			{
				return NotFound();
			}

			return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItemsList));
		}

		[HttpGet("{id}")]
		public ActionResult<PlatformReadDto> GetPlatformById(int id)
		{
			Console.WriteLine("-->Getting Platform data");
			var platformItem = _platformRepository.GetPlatformById(id);

			if (platformItem == null)
			{
				return NotFound();
			}

			return Ok(_mapper.Map<PlatformReadDto>(platformItem));
		}

		[HttpPost]
		public async Task<ActionResult<PlatformReadDto>> CreatePlatformAsync(PlatfromCreateDto model)
		{
			var platformItemModel = _mapper.Map<Platform>(model);

			_platformRepository.CreatePlatform(platformItemModel);

			_platformRepository.SaveAnyChanges();
			var platformDtoModel = _mapper.Map<PlatformReadDto>(platformItemModel);

			try
			{
				await _commandDataClient.SendPlatformToCommand(platformDtoModel);
			}
			catch (Exception ex) { }

			return Ok(platformDtoModel);

		}

	}
}
