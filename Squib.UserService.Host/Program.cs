using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Squib.UserService.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {

            return Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .UseSerilog() 
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .ConfigureAppConfiguration((_, config) =>
                        {
                            config.AddJsonFile("appsettings.host.json", true, true);
                        })
                        .UseStartup<Startup>();
                });
        }
    }
}