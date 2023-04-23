using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
	public static class PrepDb
	{
		public static void PrepPopulation(IApplicationBuilder app, bool isDevelopment)
		{
			using (var serviceScope = app.ApplicationServices.CreateScope())
			{
				SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isDevelopment);
			}
		}

		private static void SeedData(AppDbContext context, bool isDevelopment)
		{
			if (!isDevelopment)
			{
				Console.WriteLine("--> Attemting to apply migrations");
				try
				{
					context.Database.Migrate();
				}
				catch (Exception ex)
				{
					Console.WriteLine($"--> Could not run mmigrations: {ex.Message}");
				}
			
			}

			if (!context.Platforms.Any())
			{
				Console.WriteLine("-->Seeding data");
				context.Platforms.AddRange(
					new Platform() { Name = ".Net", Publisher = "Microsoft", Cost = "Free" },
					new Platform() { Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free" },
					new Platform() { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
					);

				context.SaveChanges();
			}
			else
			{
				Console.WriteLine("-->There is already data");
			}
		}
	}
}
