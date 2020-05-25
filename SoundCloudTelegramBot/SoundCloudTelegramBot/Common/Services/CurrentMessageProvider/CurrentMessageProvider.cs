using System;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SoundCloudTelegramBot.Common.Services.CurrentMessageProvider
{
    public class CurrentMessageProvider : ICurrentMessageProvider
    {
        private Update Update { get; set; }
        public UpdateType UpdateType => Update.Type;
        public CallbackQuery CallbackQuery => Update.CallbackQuery;
        public Message Message => Update.Message;

        public Chat Chat => CallbackQuery?.Message?.Chat ??
                            Message.Chat ?? throw new InvalidOperationException("There is no chat in provider.");

        public void Set(Update message)
        {
            if (Update != null)
            {
                throw new InvalidOperationException("Message is already set.");
            }

            Update = message;
        }
    }
}