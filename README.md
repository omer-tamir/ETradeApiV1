# ETradeApiV1

Rest client library for using the new E*Trade API Supports both **.NET Framework** and **.NET Core** 

## Install
Install package from [NuGet](https://www.nuget.org/packages/ETradeApi/) or Package Manager Console:

`PM> Install-Package ETrade-V1Api`

## How do I use this library?

Edit App.config and add your ConsumerKey and ConsumerSecret provided by E*Trade support team.
The repo contains a console app example that runnig the 2f authentication and get a stock price quote.
  
### Console Application Example
```
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

            ...
```
