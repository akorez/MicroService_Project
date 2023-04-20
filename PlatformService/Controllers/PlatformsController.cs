using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PlatformsController : ControllerBase
	{
		private readonly IPlatformRepository _platformRepository;
		private readonly IMapper _mapper;

		public PlatformsController(IPlatformRepository platformRepository, IMapper mapper)
		{
			_platformRepository = platformRepository;
			_mapper = mapper;
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
		public ActionResult<PlatformReadDto> CreatePlatform(PlatfromCreateDto model)
		{
			var platformItemModel = _mapper.Map<Platform>(model);

			_platformRepository.CreatePlatform(platformItemModel);

			_platformRepository.SaveAnyChanges();

			return Ok(_mapper.Map<PlatformReadDto>(platformItemModel));

		}

	}
}
