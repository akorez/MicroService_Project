using AutoMapper;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Profiles
{
	public class PlatformProfiles
	{
		public class PlatformsProfile : Profile
		{
            public PlatformsProfile()
            {
                // Source --> Target

				CreateMap<Platform,PlatformReadDto>();
				CreateMap<PlatfromCreateDto, Platform>();
				CreateMap<PlatformReadDto, PlatformPublishedDto>();
				CreateMap<Platform, GrpcPlatformModel>()
                	.ForMember(dest => dest.PlatformId, opt => opt.MapFrom(src =>src.Id))
                	.ForMember(dest => dest.Name, opt => opt.MapFrom(src =>src.Name))
                	.ForMember(dest => dest.Publisher, opt => opt.MapFrom(src =>src.Publisher));
            }
        }
	}
}
