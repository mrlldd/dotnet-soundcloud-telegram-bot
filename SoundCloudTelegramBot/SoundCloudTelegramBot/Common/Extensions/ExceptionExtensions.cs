using System;
using Telegram.Bot.Types;

namespace SoundCloudTelegramBot.Common.Extensions
{
    public static class ExceptionExtensions
    {
        public static string ToMessage(this Exception exception, Message telegramMessage)
            => exception.ToMessage() +
               $"User: {telegramMessage.From.Id} - {(telegramMessage.From.FirstName + " " + telegramMessage.From.LastName).Trim()}\n" +
               $"Username: @{telegramMessage.From.Username}\n" +
               $"Message: {telegramMessage.Text}";

        public static string ToMessage(this Exception exception, CallbackQuery callbackQuery)
            => exception.ToMessage() +
               $"User: {callbackQuery.From.Id} - {(callbackQuery.From.FirstName + " " + callbackQuery.From.LastName).Trim()}\n" +
               $"Username: @{callbackQuery.From.Username}\n" +
               $"Callback Query: {callbackQuery.Data}";

        public static string ToMessage(this Exception exception)
            => "Bot has thrown an exception:\n" +
               $"Exception: {exception.GetType()}\n" +
               $"Details: {exception.Message}\n" +
               $"Stack trace: {exception.StackTrace}\n";
    }
}