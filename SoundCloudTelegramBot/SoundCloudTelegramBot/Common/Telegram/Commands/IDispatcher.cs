using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace SoundCloudTelegramBot.Common.Telegram.Commands
{
    public interface IDispatcher
    {
        Task DispatchAsync(Update update);
        Task DispatchCommandAsync(Message message);
    }
}