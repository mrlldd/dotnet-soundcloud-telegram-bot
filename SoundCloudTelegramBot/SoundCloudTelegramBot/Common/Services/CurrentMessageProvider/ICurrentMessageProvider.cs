using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SoundCloudTelegramBot.Common.Services.CurrentMessageProvider
{
    public interface ICurrentMessageProvider
    {
        UpdateType UpdateType { get; }
        CallbackQuery CallbackQuery { get; }
        Message Message { get; }
        Chat Chat { get; }
        void Set(Update update);
    }
}