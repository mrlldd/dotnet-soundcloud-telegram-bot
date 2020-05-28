using System.Collections.Generic;
using SoundCloudTelegramBot.AppSettings.SoundCloud;
using SoundCloudTelegramBot.AppSettings.Telegram;
using Telegram.Bot.Types.Enums;

namespace SoundCloudTelegramBot.AppSettings
{
    public interface IAppConfiguration
    {
        ITelegramSettings Telegram { get; }
        ISoundCloudSettings SoundCloud { get; }
        UpdateType[] AllowedUpdates { get; } 
        string WebhookUrl { get; }
    }
}