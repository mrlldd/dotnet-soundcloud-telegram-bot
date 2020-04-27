using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SoundCloudTelegramBot.Common.Services.CurrentMessageProvider;
using Telegram.Bot.Types;

namespace SoundCloudTelegramBot.Common.Telegram.Commands
{
    public abstract class CommandBase : ICommand
    {
        protected readonly IBotProvider BotProvider;
        protected readonly ICurrentMessageProvider MessageProvider;
        private readonly IServiceProvider serviceProvider;
        public abstract string Name { get; }
        public abstract Task ExecuteAsync();

        protected CommandBase(IBotProvider botProvider, ICurrentMessageProvider messageProvider)
        {
            BotProvider = botProvider;
            MessageProvider = messageProvider;
        }
    }
}