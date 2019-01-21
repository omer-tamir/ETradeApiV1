using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using ETradeApiV1.Client.Models;

namespace ETradeApiV1.Client.Services
{
    public static class EtConfigurationService
    {
        public static EtOAuthConfig GetOAuthConfigFromSetting()
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;

            var baseUrl = settings["BaseUrl"].Value;
            var tokenUrl = settings["TokenUrl"].Value;
            var authorizeUrl = settings["AuthorizeUrl"].Value;
            var consumerKey = settings["ConsumerKey"].Value;
            var consumerSecret = settings["ConsumerSecret"].Value;
            var accessSecret = settings["AccessSecret"].Value;
            var accessToken = settings["AccessToken"].Value;

            return new EtOAuthConfig
            {
                BaseUrl = baseUrl,
                TokenUrl = tokenUrl,
                AuthorizeUrl = authorizeUrl,
                ConsumerKey = consumerKey,
                ConsumerSecret = consumerSecret,
                AccessSecret=accessSecret,
                AccessToken=accessToken

            };
        }

        public static void SaveTokenTpConfig(EtOAuthConfig config)
        {
            AddOrUpdateAppSettings("AccessToken", config.AccessToken);
            AddOrUpdateAppSettings("AccessSecret", config.AccessSecret);
        }

        private static void AddOrUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }

        
    }
}
