using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
	[Route("api/c/[controller]")]
	[ApiController]
	public class PlatformsController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly ICommandRepo _commandRepo;

		public PlatformsController(IMapper mapper, ICommandRepo commandRepo)
		{
			_mapper = mapper;
			_commandRepo = commandRepo;
		}

		[HttpGet]
		public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
		{
            Console.WriteLine("--> Getting Platforms from Command Service");

            var platformList = _commandRepo.GetlAllPlatforms();
			var result = _mapper.Map<IEnumerable<PlatformReadDto>>(platformList);

			return Ok(result);
		} 

		[HttpPost]
		public ActionResult TestInboundConnection()
		{
            Console.WriteLine("--> Inbound POST # Command Service");

			return Ok("Inbound test of Platforms Controller");
        }
	}
}
