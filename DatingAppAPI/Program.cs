using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DatingApp.Data.Models;
using Microsoft.Extensions.DependencyInjection;
using DatingApp.Data.Repository;

namespace DatingAppAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
           var host= CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var service = scope.ServiceProvider;
                try
                {
                    var context = service.GetRequiredService<DatingAppContext>();
                    Seed.SeedUsers(context);
                }
                catch (Exception ex)
                {

                    var logger = service.GetRequiredService<ILogger<Program>> ();
                    logger.LogError(ex, "An error occured");
                }
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
