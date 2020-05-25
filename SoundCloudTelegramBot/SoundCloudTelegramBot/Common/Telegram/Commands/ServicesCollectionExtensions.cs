using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using SoundCloudTelegramBot.Common.Telegram.Commands.SoundCloud.Download;
using SoundCloudTelegramBot.Common.Telegram.Commands.SoundCloud.Search;

namespace SoundCloudTelegramBot.Common.Telegram.Commands
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddTelegramCommands(this IServiceCollection services)
        {
            services.AddSingleton<IDispatcher, Dispatcher>();
            return typeof(Startup)
                .Assembly
                .GetTypes()
                .Except(new[]
                {
                    typeof(ICommand),
                    typeof(CommandBase)
                })
                .Where(x => typeof(ICommand).IsAssignableFrom(x) &&
                            x.BaseType == typeof(CommandBase))
                .Select(x => new InterfaceImplementationTypePair
                {
                    Interface = x.GetInterfaces().First(interfaceType => interfaceType != typeof(ICommand) && interfaceType.GetInterface(nameof(ICommand)) != null),
                    Implementation = x
                })
                .Aggregate(services,
                    (collection, pair) => collection.AddSingleton(pair.Interface, pair.Implementation));
        }

        private struct InterfaceImplementationTypePair
        {
            public Type Interface { get; set; }
            public Type Implementation { get; set; }
        }
    }
}