using System.Threading.Tasks;
using Telegram.Bot.Requests;

namespace SoundCloudTelegramBot.Common.Telegram.Commands
{
    public interface ICommand
    {
        string Name { get; }
        Task ExecuteAsync();
    }
}