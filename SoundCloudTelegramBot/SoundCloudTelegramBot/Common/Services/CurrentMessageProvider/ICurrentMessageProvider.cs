using Telegram.Bot.Types;

namespace SoundCloudTelegramBot.Common.Services.CurrentMessageProvider
{
    public interface ICurrentMessageProvider
    {
        Message Message { get; }
        void Set(Message message);
    }
}