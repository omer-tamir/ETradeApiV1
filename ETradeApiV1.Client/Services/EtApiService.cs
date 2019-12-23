using System;
using System.Web;
using ETradeApiV1.Client.Dtos;
using ETradeApiV1.Client.Interfaces;
using ETradeApiV1.Client.Models;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Authenticators;

namespace ETradeApiV1.Client.Services
{
    public class EtApiService : IEtApiService
    {
        private readonly EtOAuthConfig _config;

        public EtApiService(ISettingConfiguration settingConfiguration)
        {
            if (settingConfiguration == null) throw new ArgumentNullException(nameof(settingConfiguration));
            _config = settingConfiguration.GetEtradeOAuthConfig();
        }
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

            return accessToken != string.Empty;
        }

        public bool SetAccessToken(EtOAuthConfig config, string verification)
        {
            var baseUrl = new Uri(config.TokenUrl);
            var client = new RestClient(baseUrl)
            {
                Authenticator = OAuth1Authenticator.ForAccessToken(config.ConsumerKey, config.ConsumerSecret, config.OauthToken,
                    config.OauthTokenSecret, verification)
            };

            var requestAccessTokenRequest = new RestRequest("/oauth/access_token");
            var requestActionTokenResponse = client.Execute(requestAccessTokenRequest);

            var requestActionTokenResponseParameters = HttpUtility.ParseQueryString(requestActionTokenResponse.Content);

            var accessToken = requestActionTokenResponseParameters["oauth_token"];
            var accessSecret = requestActionTokenResponseParameters["oauth_token_secret"];

            config.AccessSecret = accessSecret;
            config.AccessToken = accessToken;

            return accessToken != string.Empty;
        }

        public EtOAuthConfig GetOAuthConfig()
        {
            return _config;
        }

        public IRestResponse<QuoteDto> GetQuote(string symbols, DetailFlag detailFlag = DetailFlag.ALL)
        {
            var response = GetQuote(_config, symbols, detailFlag);
            if (!response.IsSuccessful)
            {
                RenewAccessToken();
            }
            return GetQuote(_config, symbols, detailFlag);
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

        public bool RenewAccessToken()
        {
            return RenewAccessToken(_config);
        }

        public static bool RenewAccessToken(EtOAuthConfig config)
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
        public IRestResponse<AccountsListDto> ListAccounts()
        {
            var response = ListAccounts(_config);
            if (!response.IsSuccessful)
            {
                RenewAccessToken();
            }
            return ListAccounts(_config);
        }

        public static IRestResponse<AccountsListDto> ListAccounts(EtOAuthConfig config)
        {
            var qClient = new RestClient
            {
                BaseUrl = new Uri(config.BaseUrl),
                Authenticator = OAuth1Authenticator.ForProtectedResource(config.ConsumerKey, config.ConsumerSecret, config.AccessToken, config.AccessSecret)
            };
            var accountsListRequest = new RestRequest("accounts/list");
            return qClient.Execute<AccountsListDto>(accountsListRequest);
        }
    }
}
