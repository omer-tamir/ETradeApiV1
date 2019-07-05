using System;
using ETradeApiV1.Client.Interfaces;
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
                    return collection.AddSingleton<IEtApiService, EtApiService>();
                case ServiceLifetime.Transient:
                    return collection.AddTransient<IEtApiService, EtApiService>();
                default:
                    return collection.AddScoped<IEtApiService, EtApiService>();
            }
        }

        public static IServiceCollection AddEtradeService(this IServiceCollection collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            return collection.AddScoped<IEtApiService, EtApiService>();
        }
    }
}
