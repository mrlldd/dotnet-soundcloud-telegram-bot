using System;
using SoundCloudTelegramBot.Common.SoundCloud.Enums;
using SoundCloudTelegramBot.Common.SoundCloud.Models.Search;

namespace SoundCloudTelegramBot.Common.SoundCloud.Models
{
    public class CombinedEntity : IPlaylist, ITrack
    {
        public string ArtworkUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Description { get; set; }
        public DateTime DisplayDate { get; set; }
        public long Duration { get; set; }
        public string EmbeddableBy { get; set; }
        public string Genre { get; set; }
        public long Id { get; set; }
        public EntityKind Kind { get; set; }
        public string LabelName { get; set; }
        public DateTime LastModified { get; set; }
        public string License { get; set; }
        public int LikesCount { get; set; }
        public string Permalink { get; set; }
        public string PermalinkUrl { get; set; }
        public bool Public { get; set; }
        public string PurchaseTitle { get; set; }
        public string PurchaseUrl { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public int? RepostsCount { get; set; }
        public string SecretToken { get; set; }
        public string Sharing { get; set; }
        public string TagList { get; set; }
        public string Title { get; set; }
        public string Uri { get; set; }
        public SoundCloudUserModel User { get; set; }
        public long UserId { get; set; }
        public bool IsAlbum { get; set; }
        public bool ManagedByFeeds { get; set; }
        public DateTime? PublishedAt { get; set; }
        public string SetType { get; set; }
        public int TrackCount { get; set; }
        public Track[] Tracks { get; set; }
        public int CommentCount { get; set; }
        public bool Commentable { get; set; }
        public int DownloadCount { get; set; }
        public bool Downloadable { get; set; }
        public long FullDuration { get; set; }
        public bool HasDownloadsLeft { get; set; }
        public MediaModel Media { get; set; }
        public string MonetizationModel { get; set; }
        public int PlaybackCount { get; set; }
        public string Policy { get; set; }
        public PublisherMetadataModel PublisherMetadata { get; set; }
        public string State { get; set; }
        public bool Streamable { get; set; }
        public string Urn { get; set; }
        public VisualsModel Visuals { get; set; }
        public string WaveformUrl { get; set; }
    }
}