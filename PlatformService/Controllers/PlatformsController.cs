using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataService;
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
		private readonly IMessageBusClient _messageBusClient;

		public PlatformsController(IPlatformRepository platformRepository, IMapper mapper, ICommandDataClient commandDataClient, IMessageBusClient messageBusClient)
		{
			_platformRepository = platformRepository;
			_mapper = mapper;
			_commandDataClient = commandDataClient;
			_messageBusClient = messageBusClient;
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

			// Send Sync Message
			try
			{
				await _commandDataClient.SendPlatformToCommand(platformDtoModel);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
			}

			// Send Async Message
			try
			{
				var platformPublishDtoModel = _mapper.Map<PlatformPublishedDto>(platformDtoModel);
				platformPublishDtoModel.Event = "Platform_Published";
				

				_messageBusClient.PublishNewPlatform(platformPublishDtoModel);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
			}


			return Ok(platformDtoModel);

		}

	}
}
