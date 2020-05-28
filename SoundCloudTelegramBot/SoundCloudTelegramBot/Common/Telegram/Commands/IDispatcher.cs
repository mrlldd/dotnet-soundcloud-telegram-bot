using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SoundCloudTelegramBot.Common.Telegram.Commands
{
    public interface IDispatcher
    {
        Task DispatchAsync(Update update);
        IEnumerable<UpdateType> AllowedTypes { get; }
    }
}