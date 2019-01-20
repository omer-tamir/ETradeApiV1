using System;
using System.IO;
using System.Web;
using ETradeApiV1.Client.Models;
using Microsoft.Extensions.Configuration;
using RestSharp;
using RestSharp.Authenticators;

namespace ETradeApiV1.Client.Services
{
    public class ApiServices
    {

        public ApiServices()
        {
            Configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory()) // Directory where the json files are located
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .Build();

            OAuthSetting = Configuration.GetSection("OAuthSetting").Get<OAuthSetting>();
        }

        private IConfiguration Configuration { get; }
        private OAuthSetting OAuthSetting { get; set; }

        public string GetAuthorizeUrl()
        {
            var baseUrl = new Uri(OAuthSetting.BaseUrl);
            var client = new RestClient(baseUrl)
            {
                Authenticator = OAuth1Authenticator.ForRequestToken(OAuthSetting.ConsumerKey, OAuthSetting.ConsumerSecret, "oob")
            };

            var request = new RestRequest("/oauth/request_token");
            var response = client.Execute(request);

            var qs = HttpUtility.ParseQueryString(response.Content);
            var oauthToken = qs["oauth_token"];
            var oauthTokenSecret = qs["oauth_token_secret"];
            var applicationName = qs["application_name"];

            var baseSslUrl = new Uri(OAuthSetting.AuthorizeUrl);
            var sslClient = new RestClient(baseSslUrl);

            request = new RestRequest("authorize");
            request.AddParameter("key", OAuthSetting.ConsumerSecret);
            request.AddParameter("token", oauthToken);


            var url = sslClient.BuildUri(request).ToString();

            return url;
        }

    }
}
