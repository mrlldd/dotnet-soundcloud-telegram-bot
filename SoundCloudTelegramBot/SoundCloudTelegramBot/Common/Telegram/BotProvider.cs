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
        private ITelegramBotClient instance;
        private User botInfo;

        public BotProvider(ILogger<BotProvider> logger)
        {
            this.logger = logger;
        }

        public ITelegramBotClient Instance =>
            instance ?? throw new InvalidOperationException("Bot is not initialized.");

        public User Info =>
            botInfo ?? throw new InvalidOperationException("There is no bot information.");

        public async Task InitializeAsync(Func<Task<ITelegramBotClient>> provideBotAsync)
        {
            if (instance != null)
            {
                throw new InvalidOperationException("Bot is already initialized.");
            }

            instance = await provideBotAsync();
            botInfo = await instance.GetMeAsync();
            logger.LogInformation("Successfully provided bot instance.");
        }
    }
}