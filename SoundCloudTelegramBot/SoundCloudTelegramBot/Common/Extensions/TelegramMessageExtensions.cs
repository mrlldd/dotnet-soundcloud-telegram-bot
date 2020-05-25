using Telegram.Bot.Types;

namespace SoundCloudTelegramBot.Common.Extensions
{
    public static class TelegramMessageExtensions
    {
        public static bool IsCommand(this string message) => message.Trim().StartsWith("/");
    }
}