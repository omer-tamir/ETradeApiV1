using ETradeApiV1.Client.Models;

namespace ETradeApiV1.Client.Interfaces
{
    public interface IGetConfiguration
    {
        EtOAuthConfig GetConfiguration();
        Task<EtOAuthConfig> GetConfigurationAsync();
    }
}