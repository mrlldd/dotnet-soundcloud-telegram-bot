using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace SoundCloudTelegramBot.Common.Telegram.Commands
{
    public interface IDispatcher
    {
        Task DispatchCommandAsync(Message message);
    }
}