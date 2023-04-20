using PlatformService.Models;

namespace PlatformService.Data
{
	public interface IPlatformRepository
	{
		bool SaveAnyChanges();
		IEnumerable<Platform> GetAllPlatforms();
		Platform GetPlatformById(int id);
		void CreatePlatform(Platform platform);

	}
}
