using System;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace SoundCloudTelegramBot.Common.Extensions
{
    public static class LoggerExtensions
    {
        public static void LogTelegramMessage<T>(this ILogger<T> logger, Update update)
        {
            var hasLastName = string.IsNullOrEmpty(update.Message.From.LastName);
            logger.LogInformation(
                $"User {update.Message.Chat.Id} " +
                $"({update.Message.From.Username} aka " +
                $"{update.Message.From.FirstName + (hasLastName ? " " + update.Message.From.LastName : string.Empty)}" +
                $" send a message \"{update.Message.Text}\" at {update.Message.Date}.");
        }
    }
}