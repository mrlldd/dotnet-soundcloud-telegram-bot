using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SoundCloudTelegramBot.Common.Caches.Search;
using Telegram.Bot.Types;

namespace SoundCloudTelegramBot.Common.Telegram.Commands
{
    public class Dispatcher : IDispatcher
    {
        private readonly ILogger<Dispatcher> logger;
        private readonly IBotProvider botProvider;
        private readonly ISearchCache searchCache;
        private readonly Dictionary<string,ICommand> commands;
        public Dispatcher(IServiceProvider serviceProvider,
            ILogger<Dispatcher> logger,
            IBotProvider botProvider,
            ISearchCache searchCache)
        {
            this.logger = logger;
            this.botProvider = botProvider;
            this.searchCache = searchCache;
            commands = typeof(Startup)
                .Assembly
                .GetTypes()
                .Where(x => typeof(ICommand).IsAssignableFrom(x))
                .Select(serviceProvider.GetService)
                .OfType<ICommand>()
                .ToDictionary(x => x.Name);
            logger.LogInformation($"Collected {commands.Count} commands.");
        }
        public Task DispatchCommandAsync(Message message)
        {
            var (commandName, arguments) = ParseCommandText(message.Text);
            if (commandName.All(char.IsDigit) &&
                searchCache.TryGetTrackUrl(message.Chat.Id, int.Parse(commandName), out var trackUrl))
            {
                message.Text = trackUrl;
                return commands["download"].ExecuteAsync(message);
            }
            if (!commands.TryGetValue(commandName, out var command))
            {
                return botProvider.Instance.SendTextMessageAsync(message.Chat.Id, "Not found this command: " + commandName);
            }

            logger.LogInformation($"Successfully dispatched command \"{commandName}\".");
            message.Text = arguments;
            return command.ExecuteAsync(message);
        }

        private (string, string) ParseCommandText(string command)
        {
            command = command.Trim();
            var commandName = new string(command.TakeWhile(x => !x.Equals(' ')).ToArray());
            var arguments = command.Replace(commandName, string.Empty).Trim();
            return (commandName.Substring(1), arguments);
        }
    }
}