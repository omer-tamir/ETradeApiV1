using System;
using System.Configuration;
using System.Web;
using ETradeApiV1.Client.Models;
using RestSharp;
using RestSharp.Authenticators;

namespace ETradeApiV1.Client.Services
{
    public class ApiService
    {
        private EtOAuthConfig _config;

        public ApiService(EtOAuthConfig etOAuthConfig)
        {
            _config = etOAuthConfig;
        }

        public static EtOAuthConfig GetOAuthConfigFromSetting()
        {
            var baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
            var tokenUrl = ConfigurationManager.AppSettings["TokenUrl"];
            var authorizeUrl = ConfigurationManager.AppSettings["AuthorizeUrl"];
            var consumerKey = ConfigurationManager.AppSettings["ConsumerKey"];
            var consumerSecret = ConfigurationManager.AppSettings["ConsumerSecret"];
            var accessSecret = ConfigurationManager.AppSettings["AccessSecret"];
            var accessToken = ConfigurationManager.AppSettings["AccessToken"];

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

        public string GetAuthorizeUrl()
        {
            var baseUrl = new Uri(_config.TokenUrl);
            var client = new RestClient(baseUrl)
            {
                Authenticator = OAuth1Authenticator.ForRequestToken(_config.ConsumerKey, _config.ConsumerSecret, "oob")
            };

            var request = new RestRequest("/oauth/request_token");
            var response = client.Execute(request);

            var qs = HttpUtility.ParseQueryString(response.Content);
            var oauthToken = qs["oauth_token"];
            var oauthTokenSecret = qs["oauth_token_secret"];

            _config.OauthToken = oauthToken;
            _config.OauthTokenSecret = oauthTokenSecret;

            var applicationName = qs["application_name"];

            var baseSslUrl = new Uri(_config.AuthorizeUrl);
            var sslClient = new RestClient(baseSslUrl);

            request = new RestRequest("authorize");
            request.AddParameter("key", _config.ConsumerKey);
            request.AddParameter("token", oauthToken);


            var url = sslClient.BuildUri(request).ToString();

            return url;
        }

        public void GetAccessToken(string verification)
        {
            var baseUrl = new Uri(_config.TokenUrl);
            var client = new RestClient(baseUrl)
            {
                Authenticator = OAuth1Authenticator.ForAccessToken(_config.ConsumerKey, _config.ConsumerSecret, _config.OauthToken, _config.OauthTokenSecret, verification)
            };

            var requestAccessTokenRequest = new RestRequest("/oauth/access_token");
            var requestActionTokenResponse = client.Execute(requestAccessTokenRequest);

            var requestActionTokenResponseParameters = HttpUtility.ParseQueryString(requestActionTokenResponse.Content);
            var accessToken = requestActionTokenResponseParameters["oauth_token"];
            var accessSecret = requestActionTokenResponseParameters["oauth_token_secret"];

            _config.AccessSecret = accessSecret;
            _config.AccessToken = accessToken;
        }

        public EtOAuthConfig GetOAuthConfig()
        {
            return _config;
        }
    }
}
