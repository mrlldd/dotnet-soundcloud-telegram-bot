using System;
using System.Collections;
using System.Collections.Generic;
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
            var envDictionary = new Dictionary<string, string>();
            foreach (var item in Environment.GetEnvironmentVariables())
            {
                var entry = item is DictionaryEntry dictionaryEntry ? dictionaryEntry : default;
                envDictionary[entry.Key.ToString()] = entry.Value.ToString();
                logger.LogInformation($"{entry.Key} - {entry.Value}");
            }
            appConfiguration.SoundCloud.ClientId = envDictionary["CLIENTID"];
            appConfiguration.SoundCloud.OAuthToken = envDictionary["OAUTHTOKEN"];
            var bot = new TelegramBotClient(envDictionary["BOTTOKEN"]);
            logger.LogInformation(JsonConvert.SerializeObject(appConfiguration, Formatting.Indented));
            var updateRoute = "https://localhost/api/telegram/update";
            await bot.SetWebhookAsync(updateRoute);
            instance = bot;
            logger.LogInformation($"Successfully initialized bot with route: {updateRoute}");
        }
    }
}