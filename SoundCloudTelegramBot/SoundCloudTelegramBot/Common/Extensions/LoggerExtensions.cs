using System;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SoundCloudTelegramBot.Common.Extensions
{
    public static class LoggerExtensions
    {
        public static void LogTelegramMessage<T>(this ILogger<T> logger, Update update)
        {
            Message message = default;
            var sentInfo = string.Empty;
            switch (update.Type)
            {
                case UpdateType.Message:
                {
                    message = update.Message;
                    sentInfo = $"message \"{message.Text}\" at {message.Date}.";
                    break;
                }
                case UpdateType.CallbackQuery:
                {
                    var callbackQuery = update.CallbackQuery;
                    message = callbackQuery.Message;
                    sentInfo = $"callback query \"{callbackQuery.Data}\" at {message.Date}.";
                    break;
                }
            }

            if (message != default)
            {
                var hasLastName = string.IsNullOrEmpty(message.From.LastName);
                logger
                    .LogInformation($"User {message.Chat.Id} " +
                                    $"({message.From.Username} aka " +
                                    $"{message.From.FirstName + (hasLastName ? " " + message.From.LastName : string.Empty).Trim()}" +
                                    " have sent a " + sentInfo);
                return;
            }

            logger.LogInformation("There is no info in update of type: " + update.Type);
        }
    }
}