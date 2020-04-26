namespace SoundCloudTelegramBot.AppSettings
{
    public interface ISoundCloudSettings
    {
        string OAuthToken { get; }
        string ClientId { get; }
    }
}