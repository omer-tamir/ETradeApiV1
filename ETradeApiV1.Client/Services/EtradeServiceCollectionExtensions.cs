using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace ETradeApiV1.Client.Services
{
    public static class EtradeServiceCollectionExtensions
    {
        public static IServiceCollection AddEtradeService(this IServiceCollection collection,Action<EtApiServiceOptions> setupAction, ServiceLifetime contextLifetime = ServiceLifetime.Singleton)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (setupAction == null) throw new ArgumentNullException(nameof(setupAction));

            collection.Configure(setupAction);
            switch (contextLifetime)
            {
                case ServiceLifetime.Singleton:
                    return collection.AddSingleton<EtApiService, EtApiService>();
                case ServiceLifetime.Transient:
                    return collection.AddTransient<EtApiService, EtApiService>();
                default:
                    return collection.AddScoped<EtApiService, EtApiService>();
            }
        }
    }
}
