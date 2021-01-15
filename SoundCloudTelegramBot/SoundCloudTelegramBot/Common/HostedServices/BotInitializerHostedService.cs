using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SoundCloudTelegramBot.AppSettings;
using SoundCloudTelegramBot.Common.Telegram;
using SoundCloudTelegramBot.Common.Telegram.Commands;
using SoundCloudTelegramBot.Controllers;
using Telegram.Bot;
using Dispatcher = SoundCloudTelegramBot.Common.Telegram.Commands.Dispatcher;

namespace SoundCloudTelegramBot.Common.HostedServices
{
    public class BotInitializerHostedService : IHostedService
    {
        private readonly IBotProvider botProvider;
        private readonly ILogger<BotInitializerHostedService> logger;
        private readonly IAppConfiguration appConfiguration;
        private readonly IDispatcher dispatcher;

        public BotInitializerHostedService(IBotProvider botProvider,
            ILogger<BotInitializerHostedService> logger,
            IAppConfiguration appConfiguration,
            IDispatcher dispatcher)
        {
            this.botProvider = botProvider;
            this.logger = logger;
            this.appConfiguration = appConfiguration;
            this.dispatcher = dispatcher;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = logger.BeginScope("Bot initialization");
            if (await TryInitializeAutomatically())
            {
                return;
            }
#if DEBUG
            retry:
            logger.LogInformation("Input webhook URL: ");
            var input = Console.ReadLine();
            try
            {
                await botProvider.InitializeAsync(() => RegisterAsync(input));
            }
            catch (Exception e)
            {
                logger.LogError(e, "Wrong webhook: " + input);
                goto retry;
            }
#endif
        }
        
        private async Task<ITelegramBotClient> RegisterAsync(string webhookUrl)
        {
            logger.LogInformation("Started bot initialization.");
            
            var bot = new TelegramBotClient(appConfiguration.Telegram.BotToken);
            //logger.LogInformation(JsonConvert.SerializeObject(appConfiguration, Formatting.Indented));
            var routeTemplate =
                $"{webhookUrl}{typeof(TelegramController).GetCustomAttribute<RouteAttribute>()?.Template}/{nameof(TelegramController.Update).ToLower()}";
            await bot.SetWebhookAsync(routeTemplate,
                allowedUpdates: dispatcher.AllowedTypes);
            logger.LogInformation($"Successfully initialized bot with route: {routeTemplate}");
            return bot;
        }

        private async Task<bool> TryInitializeAutomatically()
        {
            logger.LogInformation("Trying to initialize bot automatically.");
            try
            {
                await botProvider.InitializeAsync(() => RegisterAsync(appConfiguration.WebhookUrl));
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed to initialize bot automatically.");
                return false;
            }

            logger.LogInformation("Successfully initialized bot automatically.");
            return true;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}