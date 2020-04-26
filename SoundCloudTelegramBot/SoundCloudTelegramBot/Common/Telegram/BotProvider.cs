using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace SoundCloudTelegramBot.Common.Telegram
{
    public class BotProvider : IBotProvider
    {
        private readonly ILogger<BotProvider> logger;
        private ITelegramBotClient instance;

        public BotProvider(ILogger<BotProvider> logger)
        {
            this.logger = logger;
        }

        public ITelegramBotClient Instance => instance ?? throw new InvalidOperationException("Bot is not initialized.");

        public async Task Initialize(string webhook)
        {
            logger.LogInformation("Started bot initialization...");
            if (instance != null)
            {
                throw new InvalidOperationException("Bot is already initialized.");
            }
            
            logger.LogInformation("Successfully initialized bot with webhook: " + webhook);
        }
    }
}