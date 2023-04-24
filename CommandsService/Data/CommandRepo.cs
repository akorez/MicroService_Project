using CommandsService.Models;

namespace CommandsService.Data
{
	public class CommandRepo : ICommandRepo
	{
		private readonly AppDbcontext _dbcontext;

		public CommandRepo(AppDbcontext dbcontext)
		{
			_dbcontext = dbcontext;
		}

		public void CreateCommand(int platformId, Command command)
		{
			if (command == null) throw new ArgumentNullException(nameof(command));

			command.PlatformId = platformId;
			_dbcontext.Commands.Add(command);

		}

		public void CreatePlatform(Platform platform)
		{
			if (platform == null) throw new ArgumentNullException(nameof(platform));

			_dbcontext.Platforms.Add(platform);
		}

		public Command GetCommand(int platformId, int commandId)
		{
			return _dbcontext.Commands.FirstOrDefault(c => c.PlatformId == platformId && c.Id == commandId);
		}

		public IEnumerable<Command> GetCommandsForPlatform(int platformId)
		{
			return _dbcontext.Commands
				.Where(c=>c.PlatformId == platformId)
				.OrderBy(c=>c.Platform.Name);
		}

		public IEnumerable<Platform> GetlAllPlatforms()
		{
			return _dbcontext.Platforms.ToList();
		}

		public bool PlatformExist(int platformId)
		{
			return _dbcontext.Platforms.Any(p=>p.Id == platformId);
		}

		public bool SaveChanges()
		{
			return (_dbcontext.SaveChanges() >= 0);
		}
	}
}
