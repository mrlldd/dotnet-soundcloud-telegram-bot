using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SoundCloudTelegramBot.AppSettings;
using Telegram.Bot;

namespace SoundCloudTelegramBot.Common.Telegram
{
    public class BotProvider : IBotProvider
    {
        private readonly ILogger<BotProvider> logger;
        private readonly IAppConfiguration appConfiguration;
        private ITelegramBotClient instance;

        public BotProvider(ILogger<BotProvider> logger, IAppConfiguration appConfiguration)
        {
            this.logger = logger;
            this.appConfiguration = appConfiguration;
        }

        public ITelegramBotClient Instance =>
            instance ?? throw new InvalidOperationException("Bot is not initialized.");

        public async Task Initialize(string webhookUrl)
        {
            logger.LogInformation("Started bot initialization.");
            if (instance != null)
            {
                throw new InvalidOperationException("Bot is already initialized.");
            }

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 |
                                                   SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
            var bot = new TelegramBotClient(appConfiguration.Telegram.BotToken);
            logger.LogInformation(JsonConvert.SerializeObject(appConfiguration, Formatting.Indented));
            var updateRoute = webhookUrl + appConfiguration.MessageUpdateRoute;
            await bot.SetWebhookAsync(updateRoute);
            instance = bot;
            logger.LogInformation($"Successfully initialized bot with route: {updateRoute}");
        }
    }
}