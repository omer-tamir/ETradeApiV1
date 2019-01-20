using System.IO;
using ETradeApiV1.Client.Models;
using Microsoft.Extensions.Configuration;

namespace ETradeApiV1.Client.Services
{
    public class ApiServices
    {
        private readonly OAuthSetting _oAuthSetting;
        public ApiServices()
        {
             Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Directory where the json files are located
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            _oAuthSetting= Configuration.GetSection("OAuthSetting").Get<OAuthSetting>();
        }

        private IConfiguration Configuration { get; }


        public OAuthSetting GetSetting()
        {
            return _oAuthSetting;
        }
    }
}
