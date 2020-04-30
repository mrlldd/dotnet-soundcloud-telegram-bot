namespace SoundCloudTelegramBot.AppSettings.SoundCloud
{
    public interface ISoundCloudSettings
    {
        string OAuthToken { get; set; }
        string ClientId { get; set; }
    }
}