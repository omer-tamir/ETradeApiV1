using System;
using ETradeApiV1.Client.Services;

namespace ETradeApiV1.Sample
{
    class Program
    {
        static void Main(string[] args)
        {


            var apiService = new ApiServices();
            var setting = apiService.GetSetting();
            Console.WriteLine($"{setting.ConsumerKey}");
            Console.ReadLine();


        }
    }
}
