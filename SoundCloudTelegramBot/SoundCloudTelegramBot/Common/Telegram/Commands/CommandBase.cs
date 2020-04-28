using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace SoundCloudTelegramBot.Common.Telegram.Commands
{
    public abstract class CommandBase : ICommand
    {
        protected readonly IBotProvider BotProvider;
        public abstract string Name { get; }
        public abstract Task ExecuteAsync(Message message);

        protected CommandBase(IBotProvider botProvider)
        {
            BotProvider = botProvider;
        }
    }
}