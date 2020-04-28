using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace SoundCloudTelegramBot.Common.Telegram.Commands
{
    public class Dispatcher : IDispatcher
    {
        private readonly ILogger<Dispatcher> logger;
        private readonly IBotProvider botProvider;
        private readonly ICommand[] commands;
        public Dispatcher(IServiceProvider serviceProvider,
            ILogger<Dispatcher> logger,
            IBotProvider botProvider)
        {
            this.logger = logger;
            this.botProvider = botProvider;
            commands = typeof(Startup)
                .Assembly
                .GetTypes()
                .Where(x => typeof(ICommand).IsAssignableFrom(x))
                .Select(serviceProvider.GetService)
                .OfType<ICommand>()
                .ToArray();
            logger.LogInformation($"Collected {commands.Length} commands.");
        }
        public Task DispatchCommandAsync(Message message)
        {
            var (commandName, arguments) = ParseCommandText(message.Text);
            var command = commands.FirstOrDefault(x => x.Name.Equals(commandName));
            if (command == null)
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