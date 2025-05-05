using Microsoft.Extensions.Configuration;

namespace Libs
{
    public interface IConfigurationUtils
    {
        string GetConfiguration(string configurationString);
    }
    public class ConfigurationUtils : IConfigurationUtils
    {
        private readonly IConfiguration _configuration;

        public ConfigurationUtils(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConfiguration(string configurationString)
        {
            return _configuration.GetSection(configurationString).Value!;
        }
    }
}
