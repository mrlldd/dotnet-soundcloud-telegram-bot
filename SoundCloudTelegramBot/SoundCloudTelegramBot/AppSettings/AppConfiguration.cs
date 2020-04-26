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
    }
}