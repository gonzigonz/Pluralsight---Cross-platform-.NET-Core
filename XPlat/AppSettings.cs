using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;

namespace XPlat
{
    public class AppSettings
    {
        private readonly Dictionary<string, string> _inMemoryDefaults;
        private readonly IConfigurationRoot _configuration;

        public AppSettings(string[] args)
        {
            _inMemoryDefaults = new Dictionary<string, string>
            {
                ["site"] = "https://g0t4.github.io/pluralsight-dotnet-core-xplat-apps"
            };

            var configBuilder = new ConfigurationBuilder()
                .AddInMemoryCollection(_inMemoryDefaults)
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddCommandLine(args)
                .AddEnvironmentVariables();

            _configuration = configBuilder.Build();

            
        }

        public string Site
        {
            get
            {
                return _configuration["site"];
            }
        }
        public OutputSettings OutputSettings
        {
            get
            {
                var outputSettings = new OutputSettings();
                _configuration.GetSection("output").Bind(outputSettings);
                return outputSettings;
            }
        }
    }
}
