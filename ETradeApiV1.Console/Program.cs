using System;
using System.Diagnostics;
using ETradeApiV1.Client.Dtos;
using ETradeApiV1.Client.Models;
using ETradeApiV1.Client.Services;
using RestSharp;
using RestSharp.Authenticators;
using System.Linq;

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
                do
                {
                    System.Console.WriteLine("1. Authenticate  ");
                    System.Console.WriteLine("2. Get Quote");
                    System.Console.WriteLine("3. Get Accounts List ");
                    System.Console.WriteLine("4. Renew access token ");
                    System.Console.WriteLine("5. Set access token ");
                    System.Console.WriteLine("Enter key: ");

                    key = System.Console.ReadLine();
                } while (key == null);

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
                    case "5":
                        SetAccessToken();
                        break;

                }
            } while (key != "0");

        }

        private static void RenewAccessToken()
        {
            var config = EtConfigurationService.GetOAuthConfigFromSetting();
            _apiServices = new EtApiService(config);
            var hasTokenRenewed = EtApiService.RenewAccessToken(config);

            System.Console.Write($"Token Renewed :{hasTokenRenewed}");
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

        private static void SetAccessToken()
        {
            string verificationKey;
            do
            {
                System.Console.Write("Enter verification key: ");

                verificationKey = System.Console.ReadLine();
            }
            while (verificationKey == null);

            var isSet = _apiServices.SetAccessToken(verificationKey);

        }

        private static void GetQuote()
        {
            var config = EtConfigurationService.GetOAuthConfigFromSetting();
            var etApiService = new EtApiService(config);

            System.Console.Write("Ticker:  ");
            var ticker = System.Console.ReadLine();
            if (ticker == "O") ticker = "TSLA:2019:02:15:PUT:220";

            var response = etApiService.GetQuote(ticker, ticker == "O" ? DetailFlag.OPTIONS : DetailFlag.ALL);

            if (response.IsSuccessful)
            {

                if (response.Data.QuoteResponse.Messages != null)
                {
                    foreach (var message in response.Data.QuoteResponse.Messages.Message)
                    {
                        System.Console.WriteLine($"{message.code} {message.description}");
                    }
                }
                else
                {
                    foreach (var item in response.Data.QuoteResponse.QuoteData)
                    {
                        System.Console.WriteLine($"{item.Product.symbol} {item.All.companyName} {item.All.lastTrade} {item.All.nextEarningDate}");
                        if(item.All.ExtendedHourQuoteDetail!=null)
                        System.Console.WriteLine($"{item.Product.symbol} {item.All.companyName} {item.All.ExtendedHourQuoteDetail.lastPrice} {item.All.nextEarningDate}");
                    }
                }
            }

            System.Console.WriteLine();
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
