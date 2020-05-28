using SoundCloudTelegramBot.AppSettings.SoundCloud;
using SoundCloudTelegramBot.AppSettings.Telegram;
using Telegram.Bot.Types.Enums;

namespace SoundCloudTelegramBot.AppSettings
{
    public class AppConfiguration : IAppConfiguration
    {
        public AppConfiguration(ITelegramSettings telegramSettings, ISoundCloudSettings soundCloudSettings)
        {
            Telegram = telegramSettings;
            SoundCloud = soundCloudSettings;
        }
        public ITelegramSettings Telegram { get; set; }
        public ISoundCloudSettings SoundCloud { get; set; }
        public UpdateType[] AllowedUpdates { get; set; }
        public string WebhookUrl { get; set; }
    }
}