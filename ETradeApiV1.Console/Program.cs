using System;
using System.Diagnostics;
using System.Web;
using ETradeApiV1.Client.Services;
using RestSharp;
using RestSharp.Authenticators;

namespace ETradeApiV1.Console
{
    class Program
    {
        private static ApiService _apiServices;
        static void Main(string[] args)
        {
            _apiServices = new ApiService();
            Authenticate_Etrade_With_Client();
        }

        private static void Authenticate_Etrade_With_Client()
        {
            
            var authorizeUrl = _apiServices.GetAuthorizeUrl();

            System.Console.WriteLine($"{authorizeUrl}");


            Process.Start(authorizeUrl);

            string verificationKey;

            do
            {
                System.Console.Write("Enter verification key: ");

                verificationKey = System.Console.ReadLine();
            }
            while (verificationKey == null);

            _apiServices.GetAccessToken(verificationKey);

            var oauthConfig=_apiServices.GetOAuthConfig();
            var qClient = new RestClient
            {
                BaseUrl = new Uri("https://apisb.etrade.com/v1/accounts/"),
                Authenticator = OAuth1Authenticator.ForProtectedResource(oauthConfig.ConsumerKey, oauthConfig.ConsumerSecret, oauthConfig.AccessToken, oauthConfig.AccessSecret)

            };

            var accountsListRequest = new RestRequest("list");
            var accountsListResponse = qClient.Execute(accountsListRequest);
        }
   
    }
}
