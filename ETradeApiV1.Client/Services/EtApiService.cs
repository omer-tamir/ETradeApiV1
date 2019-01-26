using System;
using System.Configuration;
using System.Web;
using ETradeApiV1.Client.Dtos;
using ETradeApiV1.Client.Models;
using RestSharp;
using RestSharp.Authenticators;

namespace ETradeApiV1.Client.Services
{
    public class EtApiService
    {
        private readonly EtOAuthConfig _config;

        public EtApiService(EtOAuthConfig etOAuthConfig)
        {
            _config = etOAuthConfig;
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

        public bool SetAccessToken(string verification)
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

            return accessToken != String.Empty;
        }

        public EtOAuthConfig GetOAuthConfig()
        {
            return _config;
        }

        public static IRestResponse<QuoteDto> GetQuote(EtOAuthConfig config, string symbols, DetailFlag detailFlag = DetailFlag.ALL)
        {
            var qClient = new RestClient
            {
                BaseUrl = new Uri(config.BaseUrl),
                Authenticator = OAuth1Authenticator.ForProtectedResource(config.ConsumerKey, config.ConsumerSecret,
                    config.AccessToken, config.AccessSecret)
            };

            var request = new RestRequest($"market/quote/{symbols}");
            request.AddQueryParameter("detailFlag", detailFlag.ToString());
            request.AddQueryParameter("requireEarningsDate", "true");
            request.AddQueryParameter("skipMiniOptionsCheck", "true");
            var response = qClient.Execute<QuoteDto>(request);
            return response;
        }

        public bool RenewAccessToken(EtOAuthConfig config)
        {
            var client = new RestClient
            {
                BaseUrl = new Uri(config.TokenUrl),
                Authenticator = OAuth1Authenticator.ForProtectedResource(config.ConsumerKey, config.ConsumerSecret, config.AccessToken, config.AccessSecret)
            };

            var request = new RestRequest($"/oauth/renew_access_token");
            var response = client.Execute(request);

            return response.Content == "Access Token has been renewed";
        }
    }
}
