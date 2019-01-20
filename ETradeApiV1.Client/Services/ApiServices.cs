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

        public ApiService()
        {
            GetOAuthFromSetting();
        }
        private OAuthConfig OAuthConfig { get; set; }

        private void GetOAuthFromSetting()
        {
            var baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
            var authorizeUrl = ConfigurationManager.AppSettings["AuthorizeUrl"];
            var consumerKey = ConfigurationManager.AppSettings["ConsumerKey"];
            var consumerSecret = ConfigurationManager.AppSettings["ConsumerSecret"];

            OAuthConfig = new OAuthConfig
            {
                BaseUrl=baseUrl,
                AuthorizeUrl=authorizeUrl,
                ConsumerKey=consumerKey,
                ConsumerSecret= consumerSecret
            };
        }

        public string GetAuthorizeUrl()
        {
            var baseUrl = new Uri(OAuthConfig.BaseUrl);
            var client = new RestClient(baseUrl)
            {
                Authenticator = OAuth1Authenticator.ForRequestToken(OAuthConfig.ConsumerKey, OAuthConfig.ConsumerSecret, "oob")
            };

            var request = new RestRequest("/oauth/request_token");
            var response = client.Execute(request);

            var qs = HttpUtility.ParseQueryString(response.Content);
            var oauthToken = qs["oauth_token"];
            var oauthTokenSecret = qs["oauth_token_secret"];

            OAuthConfig.OauthToken = oauthToken;
            OAuthConfig.OauthTokenSecret = oauthTokenSecret;

            var applicationName = qs["application_name"];

            var baseSslUrl = new Uri(OAuthConfig.AuthorizeUrl);
            var sslClient = new RestClient(baseSslUrl);

            request = new RestRequest("authorize");
            request.AddParameter("key", OAuthConfig.ConsumerKey);
            request.AddParameter("token", oauthToken);


            var url = sslClient.BuildUri(request).ToString();

            return url;
        }

        public void GetAccessToken(string verification)
        {
            var baseUrl = new Uri("https://apisb.etrade.com");
            var client = new RestClient(baseUrl)
            {
                Authenticator = OAuth1Authenticator.ForAccessToken(OAuthConfig.ConsumerKey, OAuthConfig.ConsumerSecret,OAuthConfig.OauthToken, OAuthConfig.OauthTokenSecret, verification)
            };
        
            var requestAccessTokenRequest = new RestRequest("/oauth/access_token");
            var requestActionTokenResponse = client.Execute(requestAccessTokenRequest);

            var requestActionTokenResponseParameters = HttpUtility.ParseQueryString(requestActionTokenResponse.Content);
            var accessToken = requestActionTokenResponseParameters["oauth_token"];
            var accessSecret = requestActionTokenResponseParameters["oauth_token_secret"];

            OAuthConfig.AccessSecret=accessSecret;
            OAuthConfig.AccessToken = accessToken;
        }

        public OAuthConfig GetOAuthConfig()
        {
            return OAuthConfig;
        }
    }
}
