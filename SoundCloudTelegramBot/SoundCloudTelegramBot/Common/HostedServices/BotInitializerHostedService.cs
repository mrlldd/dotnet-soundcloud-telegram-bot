using System;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SoundCloudTelegramBot.AppSettings;
using SoundCloudTelegramBot.Common.Telegram;

namespace SoundCloudTelegramBot.Common.HostedServices
{
    public class BotInitializerHostedService : IHostedService
    {
        private readonly IBotProvider botProvider;
        private readonly ILogger<BotInitializerHostedService> logger;

        public BotInitializerHostedService(IBotProvider botProvider, ILogger<BotInitializerHostedService> logger)
        {
            this.botProvider = botProvider;
            this.logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = logger.BeginScope("Bot initialization");
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
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}