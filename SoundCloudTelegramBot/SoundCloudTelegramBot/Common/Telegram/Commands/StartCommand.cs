using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace SoundCloudTelegramBot.Common.Telegram.Commands
{
    public class StartCommand : CommandBase, IStartCommand
    {
        public StartCommand(IBotProvider botProvider) : base(botProvider)
        {
        }

        public override string Name => "start";
        public override Task ExecuteAsync(Message message) => Task.CompletedTask;
    }
}