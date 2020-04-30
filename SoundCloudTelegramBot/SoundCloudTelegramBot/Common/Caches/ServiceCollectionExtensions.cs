using Microsoft.Extensions.DependencyInjection;
using SoundCloudTelegramBot.Common.Caches.Search;

namespace SoundCloudTelegramBot.Common.Caches
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCaches(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<ISearchCache, SearchCache>();
            return services;
        }
    }
}