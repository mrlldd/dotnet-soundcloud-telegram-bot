using SoundCloudTelegramBot.AppSettings.SoundCloud;
using SoundCloudTelegramBot.AppSettings.Telegram;

namespace SoundCloudTelegramBot.AppSettings
{
    public interface IAppConfiguration
    {
        ITelegramSettings Telegram { get; }
        ISoundCloudSettings SoundCloud { get; }
        string MessageUpdateRoute { get; }
        string WebhookUrl { get; }
    }
}