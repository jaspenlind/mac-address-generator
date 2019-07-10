using System;
using System.IO;
using MacAddressGenerator.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Lamar;

namespace MacAddressGenerator
{
    class Program
    {
        public static IConfigurationRoot Configuration { get; set; }

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            var container = new Container(x =>
            {
                x.AddLogging(x => x.AddConsole());
                x.Configure<OUI>(Configuration.GetSection("oui"));
                x.Scan(y =>
                {
                    y.TheCallingAssembly();
                    y.WithDefaultConventions();
                });
            });

            var service = container.GetService<IMacAddressService>();

            var mac = service.Generate();

            Clipboard.Copy(mac);

            var logger = container.GetService<ILogger<Program>>();

            logger.LogInformation($"A new mac address '{mac}' has been generated and added to your clipboard!");
        }
    }
}
