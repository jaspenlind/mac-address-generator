using System;
using System.IO;
using MacAddressGenerator.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StructureMap;

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

            var services = new ServiceCollection();

            var serviceProvider = ConfigureServices(services);

            var service = serviceProvider.GetService<IMacAddressService>();

            var mac = service.Generate();

            Clipboard.Copy(mac);

            var logger = serviceProvider.GetService<ILogger<Program>>();

            logger.LogInformation($"A new mac address '{mac}' has been generated and added to your clipboard!");
        }

        private static IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(x => x.AddConsole());

            services.Configure<OUI>(Configuration.GetSection("oui"));

            var container = IOC.Current;

            container.Populate(services);

            new StructureMapConfig(container).Bootstrap();

            return container.GetInstance<IServiceProvider>();
        }
    }
}
