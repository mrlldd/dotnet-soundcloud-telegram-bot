using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SoundCloudTelegramBot.AppSettings;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SoundCloudTelegramBot.Common.Telegram
{
    public class BotProvider : IBotProvider
    {
        private readonly ILogger<BotProvider> logger;
        private readonly IAppConfiguration appConfiguration;
        private ITelegramBotClient instance;
        private User botInfo;
        public BotProvider(ILogger<BotProvider> logger, IAppConfiguration appConfiguration)
        {
            this.logger = logger;
            this.appConfiguration = appConfiguration;
        }

        public ITelegramBotClient Instance =>
            instance ?? throw new InvalidOperationException("Bot is not initialized.");

        public User BotInfo => 
            botInfo ?? throw new InvalidOperationException("There is no bot information.");
        public async Task Initialize(string webhookUrl)
        {
            logger.LogInformation("Started bot initialization.");
            if (instance != null)
            {
                throw new InvalidOperationException("Bot is already initialized.");
            }
            
            var bot = new TelegramBotClient(appConfiguration.Telegram.BotToken);
            //logger.LogInformation(JsonConvert.SerializeObject(appConfiguration, Formatting.Indented));
            var updateRoute = webhookUrl + appConfiguration.MessageUpdateRoute;
            await bot.SetWebhookAsync(updateRoute);
            botInfo = await bot.GetMeAsync();
            instance = bot;
            logger.LogInformation($"Successfully initialized bot with route: {updateRoute}");
        }
    }
}