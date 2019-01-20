using System;
using System.Configuration;
using System.Diagnostics;
using System.Web;
using ETradeApiV1.Models;
using RestSharp;
using RestSharp.Authenticators;

namespace ETradeApiV1.Services
{
    public class ApiServices
    {

        public ApiServices()
        {
            GetOAuthFromSetting();
        }

        private void GetOAuthFromSetting()
        {
            var baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
            var authorizeUrl = ConfigurationManager.AppSettings["AuthorizeUrl"];
            var consumerKey = ConfigurationManager.AppSettings["ConsumerKey"];
            var consumerSecret = ConfigurationManager.AppSettings["ConsumerSecret"];

            OAuthSetting = new OAuthSetting
            {
                BaseUrl=baseUrl,
                AuthorizeUrl=authorizeUrl,
                ConsumerKey=consumerKey,
                ConsumerSecret= consumerSecret
            };
        }


        private OAuthSetting OAuthSetting { get; set; }

        public string GetAuthorizeUrl()
        {
            var baseUrl = new Uri("https://apisb.etrade.com");
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

            var baseSslUrl = new Uri("https://us.etrade.com/e/t/etws/");
            var sslClient = new RestClient(baseSslUrl);

            request = new RestRequest("authorize");
            request.AddParameter("key", OAuthSetting.ConsumerSecret);
            request.AddParameter("token", oauthToken);


            var url = sslClient.BuildUri(request).ToString();

            return url;
        }

        public static void Authenticate_Etrade_With_OAuth()
        {
            const string consumerKey = "c9c23e8383b8e1ed78d570b0e152bfdb";
            const string consumerSecret = "9c3d9420641d6897c3a5b0e51d6430f1";

            var baseUrl = new Uri("https://apisb.etrade.com");
            var client = new RestClient(baseUrl)
            {
                Authenticator = OAuth1Authenticator.ForRequestToken(consumerKey, consumerSecret, "oob")

            };

            var request = new RestRequest("/oauth/request_token");
            var response = client.Execute(request);

            var qs = HttpUtility.ParseQueryString(response.Content);
            var oauthToken = qs["oauth_token"];
            var oauthTokenSecret = qs["oauth_token_secret"];
            var applicationName = qs["application_name"];

            var baseSslUrl = new Uri("https://us.etrade.com/e/t/etws/");
            var sslClient = new RestClient(baseSslUrl);

            request = new RestRequest("authorize");
            request.AddParameter("key", consumerKey);
            request.AddParameter("token", oauthToken);


            var url = sslClient.BuildUri(request).ToString();

            Process.Start(url);

            string verificationKey;

            do
            {
                System.Console.Write("Enter verification key: ");

                verificationKey = System.Console.ReadLine();
            }
            while (verificationKey == null);


            client.Authenticator = OAuth1Authenticator.ForAccessToken(consumerKey, consumerSecret, oauthToken, oauthTokenSecret, verificationKey);
            var requestAccessTokenRequest = new RestRequest("/oauth/access_token");
            var requestActionTokenResponse = client.Execute(requestAccessTokenRequest);

            var requestActionTokenResponseParameters = HttpUtility.ParseQueryString(requestActionTokenResponse.Content);
            var accessToken = requestActionTokenResponseParameters["oauth_token"];
            var accessSecret = requestActionTokenResponseParameters["oauth_token_secret"];

            var qClient = new RestClient
            {
                BaseUrl = new Uri("https://apisb.etrade.com/v1/accounts/"),
                Authenticator = OAuth1Authenticator.ForProtectedResource(consumerKey, consumerSecret, accessToken, accessSecret)

            };

            var accountsListRequest = new RestRequest("list");
            var accountsListResponse = qClient.Execute(accountsListRequest);

        }

    }
}
