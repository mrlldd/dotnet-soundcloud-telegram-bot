using SoundCloudTelegramBot.AppSettings.SoundCloud;
using SoundCloudTelegramBot.AppSettings.Telegram;

namespace SoundCloudTelegramBot.AppSettings
{
    public class AppConfiguration : IAppConfiguration
    {
        public AppConfiguration()
        {
            Telegram = new TelegramSettings();
            SoundCloud = new SoundCloudSettings();
        }
        public ITelegramSettings Telegram { get; set; }
        public ISoundCloudSettings SoundCloud { get; set; }

        public string MessageUpdateRoute { get; set; }
        public string WebhookUrl { get; set; }
    }
}