using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using MacAddressGenerator.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MacAddressGenerator
{
    public interface IMacAddressService
    {
        string Generate();
    }

    public class MacAddressService : IMacAddressService
    {
        private static readonly Random Random = new Random();

        private readonly ILogger<MacAddressService> logger;

        private readonly OUI oui;

        public MacAddressService(ILoggerFactory loggerFactory, IOptions<OUI> oui)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            logger = loggerFactory.CreateLogger<MacAddressService>();

            this.oui = oui?.Value ?? throw new ArgumentNullException(nameof(oui));
        }

        public string Generate()
        {
            var mac = GetRandomMacAddress();

            return mac;
        }

        private string GetRandomMacAddress()
        {
            var buffer = new byte[6];

            var ouiPrefix = string.Empty;

            if (oui.Enabled && !string.IsNullOrWhiteSpace(oui.Value))
            {
                var definition = GetRandomOui();

                logger.LogInformation($"Using '{definition.Oui}' for manufacturer '{definition.Manufacturer}' as Organizationally Unique Identifier (OUI)");

                buffer = new byte[3];

                ouiPrefix = $"{definition.Oui}:";
            }

            Random.NextBytes(buffer);

            return $"{ouiPrefix}{BitConverter.ToString(buffer)}".Replace("-", ":");
        }

        private static OuiDefinition GetRandomOui()
        {
            IEnumerable<OuiDefinition> allDefinitions;

            using (var reader = File.OpenText("oui.csv"))
            {
                var csv = new CsvReader(reader);

                csv.Configuration.HasHeaderRecord = false;

                allDefinitions = csv.GetRecords<OuiDefinition>().ToList();
            }

            return allDefinitions.ElementAt(Random.Next(0, allDefinitions.Count() - 1));
        }
    }
}
