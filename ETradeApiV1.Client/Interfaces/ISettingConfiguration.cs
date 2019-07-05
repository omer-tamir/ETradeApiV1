using ETradeApiV1.Client.Models;

namespace ETradeApiV1.Client.Interfaces
{
    public interface ISettingConfiguration
    {
        EtOAuthConfig GetEtradeOAuthConfig();
    }
}