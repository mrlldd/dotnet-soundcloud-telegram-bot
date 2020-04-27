using System;
using Telegram.Bot.Types;

namespace SoundCloudTelegramBot.Common.Services.CurrentMessageProvider
{
    public class CurrentMessageProvider : ICurrentMessageProvider
    {
        public Message Message { get; private set; }
        public void Set(Message message)
        {
            if (Message != null)
            {
                throw new InvalidOperationException("Message is already set.");
            }

            Message = message;
        }
    }
}