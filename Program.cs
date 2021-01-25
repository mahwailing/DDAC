using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore; //sql action
using Microsoft.Extensions.DependencyInjection; //extension
using MVCFlowerShopDDACLAB3.Data; //data link
using MVCFlowerShopDDACLAB3.Models; //link to seeddata class

namespace MVCFlowerShopDDACLAB3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();
            var host = CreateHostBuilder(args).Build();
            //link with my initialize() method in seeddata.cs
            //host.Run();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider; //create service object 
                try
                {
                    var context = services.GetRequiredService<MVCFlowerShopDDACLAB3NewContext>();
                    context.Database.Migrate();
                    SeednData.Initialize(services); //call your function
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    //run in output
                    logger.LogError(ex, "An error occured seeding the DB.");
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
