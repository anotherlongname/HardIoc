using Microsoft.Extensions.Configuration;

namespace WebApplication1.Handlers
{
    public delegate string ReadConfiguration();

    public class ReadConfigurationHandler
    {
        private readonly IConfiguration _configuration;

        public ReadConfigurationHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Handle()
        {
            return _configuration.GetValue<string>("SettingValue");
        }
    }
}
