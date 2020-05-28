using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using FastExpressionCompiler;
using Microsoft.Extensions.Logging;
using SoundCloudTelegramBot.Common.Caches.Search;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace SoundCloudTelegramBot.Common.Telegram.Commands
{
    public class Dispatcher : IDispatcher
    {
        private readonly ILogger<Dispatcher> logger;
        private readonly IBotProvider botProvider;
        private readonly IReadOnlyDictionary<string, Func<Message, Task>> commands;
        private readonly IReadOnlyDictionary<UpdateType, Func<Update, Task>> updateHandlers;
        public IEnumerable<UpdateType> AllowedTypes => updateHandlers.Keys;
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
                .ToDictionary<ICommand, string, Func<Message, Task>>(x => x.Name, x => x.ExecuteAsync);
            var parameter = Expression.Parameter(typeof(Update));
            updateHandlers = typeof(Dispatcher)
                    .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                    .Where(x => x.GetCustomAttribute<UpdateHandlerAttribute>() != null)
                    .ToDictionary(x => (UpdateType)Enum.Parse(typeof(UpdateType), x.Name),
                    x => Expression
                        .Lambda<Func<Update, Task>>(
                            Expression.Call(Expression.Constant(this), x, parameter),
                            parameter).CompileFast());
            logger.LogInformation($"Collected {commands.Count} commands.");
        }

        public Task DispatchAsync(Update update)
            => updateHandlers[update.Type](update);
        
        [UpdateHandler]
        private Task Message(Update update) => DispatchCommandAsync(update.Message);

        public Task DispatchCommandAsync(Message message)
        {
            var (commandName, arguments) = ParseCommandText(message.Text);
            if (commandName.All(char.IsDigit))
            {
                message.Text = commandName;
                return commands["resolve"](message);
            }

            if (!commands.TryGetValue(commandName, out var command))
            {
                return botProvider.Instance.SendTextMessageAsync(message.Chat.Id,
                    "Not found this command: " + commandName);
            }

            logger.LogInformation($"Successfully dispatched command \"{commandName}\" with arguments {arguments}.");
            message.Text = arguments;
            return command(message);
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