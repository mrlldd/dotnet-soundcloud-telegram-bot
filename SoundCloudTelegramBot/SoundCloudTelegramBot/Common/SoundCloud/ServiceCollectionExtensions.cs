using Microsoft.Extensions.DependencyInjection;
using SoundCloudTelegramBot.Common.SoundCloud.Interaction;

namespace SoundCloudTelegramBot.Common.SoundCloud
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSoundCloudServices(this IServiceCollection services)
        {
            services.AddSingleton<ISoundCloudInteractor, SoundCloudInteractor>();
            return services;
        }
    }
}