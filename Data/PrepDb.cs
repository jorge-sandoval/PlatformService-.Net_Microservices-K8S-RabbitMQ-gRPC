using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using ( var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<AppDbContext>();
                
                if (dbContext != null)
                {
                    SeedData(dbContext, env);
                }
            }
        }

        private static void SeedData(AppDbContext context, IWebHostEnvironment env)
        {
            if(env.IsProduction())
            {
                Console.WriteLine("--> Attempting to Apply Migrations");
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"--> Could not run migrations: {e.Message}");
                }
            }

            if ( !context.PlatForms.Any() )
            {
                Console.WriteLine("--> Seeding Data");

                context.PlatForms.AddRange(
                    new Platform() { Name = "Dot Net", Publisher = "Microsoft", Cost = "Free" },
                    new Platform() { Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free" },
                    new Platform() { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
                );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> We already have data");
            }
        }
    }
}
