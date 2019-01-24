using System;
using System.Diagnostics;
using ETradeApiV1.Client.Dtos;
using ETradeApiV1.Client.Services;
using RestSharp;
using RestSharp.Authenticators;

namespace ETradeApiV1.Console
{
    class Program
    {
        private static EtApiService _apiServices;
        static void Main(string[] args)
        {
            string key;
            do
            {
                System.Console.WriteLine("1. Authenticate  ");
                System.Console.WriteLine("2. Get Quote");
                System.Console.WriteLine("3. Get Accounts List ");
                System.Console.WriteLine("4. Renew access token ");
                System.Console.WriteLine("Enter key: ");

                key = System.Console.ReadLine();
            }
            while (key == null);

            switch (key)
            {
                case "1":
                    Authenticate_Etrade_With_Client();
                    break;
                case "2":
                    GetQuote();
                    break;
                case "3":
                    GetAccountsList();
                    break;
                case "4":
                    RenewAccessToken();
                    break;


            }


        }

        private static void RenewAccessToken()
        {
            var config = EtConfigurationService.GetOAuthConfigFromSetting();
            _apiServices = new EtApiService(config);
            var hasTokenRenewed = _apiServices.RenewAccessToken(config);
            
            System.Console.Write($"{hasTokenRenewed}");

            System.Console.ReadLine();
        }

        private static void Authenticate_Etrade_With_Client()
        {

            var config = EtConfigurationService.GetOAuthConfigFromSetting();
            _apiServices = new EtApiService(config);

            var authorizeUrl = _apiServices.GetAuthorizeUrl();
            Process.Start(authorizeUrl);

            string verificationKey;

            do
            {
                System.Console.Write("Enter verification key: ");

                verificationKey = System.Console.ReadLine();
            }
            while (verificationKey == null);

            var isSet = _apiServices.SetAccessToken(verificationKey);
            if (isSet) EtConfigurationService.SaveTokenTpConfig(config);


        }

        private static void GetQuote()
        {
            var config = EtConfigurationService.GetOAuthConfigFromSetting();
            var response = EtApiService.GetQuote(config, "CVS,T");

            foreach (var item in response.Data.QuoteResponse.QuoteData)
            {
                System.Console.WriteLine($"{item.Product.symbol} {item.Product.securityType}");
            }

            System.Console.ReadLine();
        }


        private static void GetAccountsList()
        {
            var config = EtConfigurationService.GetOAuthConfigFromSetting();
            var qClient = new RestClient
            {
                BaseUrl = new Uri(config.BaseUrl),
                Authenticator = OAuth1Authenticator.ForProtectedResource(config.ConsumerKey, config.ConsumerSecret, config.AccessToken, config.AccessSecret)

            };

            var accountsListRequest = new RestRequest("accounts/list");
            var response = qClient.Execute<AccountsListDto>(accountsListRequest);

            foreach (var item in response.Data.AccountListResponse.Accounts.Account)
            {
                System.Console.WriteLine($"{item.AccountName} {item.AccountStatus}");
            }

            System.Console.ReadLine();
        }

    }
}
