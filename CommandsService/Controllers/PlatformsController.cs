using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
	[Route("api/c/[controller]")]
	[ApiController]
	public class PlatformsController : ControllerBase
	{
		private readonly IMapper _mapper;

		public PlatformsController(IMapper mapper)
		{
			_mapper = mapper;
		}

		[HttpPost]
		public ActionResult TestInboundConnection()
		{
            Console.WriteLine("--> Inbound POST # Command Service");

			return Ok("Inbound test of Platforms Controller");
        }
	}
}
