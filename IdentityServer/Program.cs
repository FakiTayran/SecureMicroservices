using AspNetCore.Identity.Dapper.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

            //using (var scope = host.Services.CreateScope())
            //{
            //    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<CustomIdentityUser>>();
            //    var user = new CustomIdentityUser
            //    { 
            //        UserName = "tayran.ariduru@nilvera.com",
            //        Email = "tayran.ariduru@nilvera.com",
            //    };

            //    userManager.CreateAsync(user, "Password1.").GetAwaiter().GetResult();
            //}

            //    host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
             .ConfigureAppConfiguration((host, config) => {
                 config.AddJsonFile("ocelot.json");
             })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
