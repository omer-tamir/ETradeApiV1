using ETradeApiV1.Client.Dtos;
using ETradeApiV1.Client.Models;
using RestSharp;

namespace ETradeApiV1.Client.Interfaces
{
    public interface IEtApiService
    {
        string GetAuthorizeUrl();
        bool SetAccessToken(string verification);
        bool SetAccessToken(EtOAuthConfig config, string verification);
        EtOAuthConfig GetOAuthConfig();
        IRestResponse<QuoteDto> GetQuote(string symbols, DetailFlag detailFlag = DetailFlag.ALL);
        bool RenewAccessToken();
    }
}