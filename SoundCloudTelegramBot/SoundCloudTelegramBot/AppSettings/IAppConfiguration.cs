namespace SoundCloudTelegramBot.AppSettings
{
    public interface IAppConfiguration
    {
        ITelegramSettings Telegram { get; }
        ISoundCloudSettings SoundCloud { get; }
    }
}