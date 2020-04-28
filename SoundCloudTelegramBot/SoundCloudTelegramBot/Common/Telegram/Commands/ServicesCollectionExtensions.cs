using Microsoft.Extensions.DependencyInjection;
using SoundCloudTelegramBot.Common.Telegram.Commands.SoundCloud.Search;

namespace SoundCloudTelegramBot.Common.Telegram.Commands
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddTelegramCommands(this IServiceCollection services)
        {
            services.AddSingleton<ISearchTracksCommand, SearchTracksCommand>();
            
            services.AddSingleton<IDispatcher, Dispatcher>();
            
            
            return services;
        }
    }
}