namespace SoundCloudTelegramBot.Common.SoundCloud.Models
{
    public interface ITrack : ITypedEntity
    {
        int CommentCount { get; set; }
        bool Commentable { get; set; }
        int DownloadCount { get; set; }
        bool Downloadable { get; set; }
        long FullDuration { get; set; }
        bool HasDownloadsLeft { get; set; }
        MediaModel Media { get; set; }
        string MonetizationModel { get; set; }
        int PlaybackCount { get; set; }
        string Policy { get; set; }
        PublisherMetadataModel PublisherMetadata { get; set; }
        string State { get; set; }
        bool Streamable { get; set; }
        string Urn { get; set; }
        VisualsModel Visuals { get; set; }
        string WaveformUrl { get; set; }
    }
}