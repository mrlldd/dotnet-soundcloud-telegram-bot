using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SoundCloudTelegramBot.AppSettings;
using SoundCloudTelegramBot.Common.Telegram;

namespace SoundCloudTelegramBot.Common.HostedServices
{
    public class BotInitializerHostedService : IHostedService
    {
        private readonly IBotProvider botProvider;
        private readonly ILogger<BotInitializerHostedService> logger;
        private readonly IAppConfiguration appConfiguration;

        public BotInitializerHostedService(IBotProvider botProvider, ILogger<BotInitializerHostedService> logger,
            IAppConfiguration appConfiguration)
        {
            this.botProvider = botProvider;
            this.logger = logger;
            this.appConfiguration = appConfiguration;
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
                await botProvider.Initialize(input);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Wrong webhook: " + input);
                goto retry;
            }
#endif
        }

        private async Task<bool> TryInitializeAutomatically()
        {
            logger.LogInformation("Trying to initialize bot automatically.");
            try
            {
                await botProvider.Initialize("https://soundcloud-in-play-tg-bot.herokuapp.com/");
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