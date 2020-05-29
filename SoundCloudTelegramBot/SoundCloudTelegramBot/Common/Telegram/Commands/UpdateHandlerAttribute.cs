using System;
using Telegram.Bot.Types.Enums;

namespace SoundCloudTelegramBot.Common.Telegram.Commands
{
    public class UpdateHandlerAttribute : Attribute
    {
        public UpdateType UpdateType { get; }

        public UpdateHandlerAttribute(UpdateType updateType)
        {
            UpdateType = updateType;
        }
    }
}