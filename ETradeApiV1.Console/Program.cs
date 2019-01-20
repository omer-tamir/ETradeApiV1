using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETradeApiV1.Client.Services;

namespace ETradeApiV1.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var apiService = new ApiServices();
            var authorizeUrl = apiService.GetAuthorizeUrl();

            System.Console.WriteLine($"{authorizeUrl}");


            Process.Start(authorizeUrl);

            string verificationKey;

            do
            {
                System.Console.Write("Enter verification key: ");

                verificationKey = System.Console.ReadLine();
            }
            while (verificationKey == null);
        }
    }
}
