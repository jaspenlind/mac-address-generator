using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Lamar;

namespace MacAddressGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var switchMappings = new Dictionary<string, string>()
             {
                 { "-oui", "oui" }
             };

            var builder = new ConfigurationBuilder().AddCommandLine(args, switchMappings);

            var config = builder.Build();

            var container = new Container(x =>
            {
                x.AddLogging(x => x.AddConsole());
                x.Configure((Action<Config>)(x => x.OUI = config[(string)"oui"]?.Truncate(8)));
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
