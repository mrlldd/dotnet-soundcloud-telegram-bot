namespace SoundCloudTelegramBot.AppSettings.SoundCloud
{
    public interface ISoundCloudSettings
    {
        string OAuthToken { get; }
        string ClientId { get; }
    }
}