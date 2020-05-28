using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SoundCloudTelegramBot.AppSettings;
using SoundCloudTelegramBot.Controllers;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

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

        public User Info =>
            botInfo ?? throw new InvalidOperationException("There is no bot information.");

        public async Task InitializeAsync(string webhookUrl)
        {
            logger.LogInformation("Started bot initialization.");
            if (instance != null)
            {
                throw new InvalidOperationException("Bot is already initialized.");
            }

            var bot = new TelegramBotClient(appConfiguration.Telegram.BotToken);
            //logger.LogInformation(JsonConvert.SerializeObject(appConfiguration, Formatting.Indented));
            var routeTemplate = webhookUrl + typeof(TelegramController)
                                    .GetCustomAttribute<RouteAttribute>()
                                    .Template + $"/{nameof(TelegramController.Update).ToLower()}";
            await bot.SetWebhookAsync(routeTemplate,
                allowedUpdates: appConfiguration.AllowedUpdates);
            botInfo = await bot.GetMeAsync();
            instance = bot;
            logger.LogInformation($"Successfully initialized bot with route: {routeTemplate}");
        }
    }
}