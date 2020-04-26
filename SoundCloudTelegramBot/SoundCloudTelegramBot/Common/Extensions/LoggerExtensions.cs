using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace SoundCloudTelegramBot.Common.Extensions
{
    public static class LoggerExtensions
    {
        public static void LogTelegramMessage<T>(this ILogger<T> logger, Update update)
        {
            logger.LogInformation(
                $"User {update.Message.Chat.Id} " +
                $"({update.Message.From.Username} aka {update.Message.From.FirstName + update.Message.From.LastName})" +
                $" send a message \"{update.Message.Text}\" at {update.Message.Date}.");
        }
    }
}