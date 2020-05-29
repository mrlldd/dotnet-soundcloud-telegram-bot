using System;
using SoundCloudTelegramBot.Common.SoundCloud.Enums;

namespace SoundCloudTelegramBot.Common.SoundCloud.Models
{
    public interface ITypedEntity
    {
        string ArtworkUrl { get; set; }
        DateTime CreatedAt { get; set; }
        string Description { get; set; }
        DateTime DisplayDate { get; set; }
        long Duration { get; set; }
        string EmbeddableBy { get; set; }
        string Genre { get; set; }
        long Id { get; set; }
        EntityKind Kind { get; set; }
        string LabelName { get; set; }
        DateTime LastModified { get; set; }
        string License { get; set; }
        int LikesCount { get; set; }
        string Permalink { get; set; }
        string PermalinkUrl { get; set; }
        bool Public { get; set; }
        string PurchaseTitle { get; set; }
        string PurchaseUrl { get; set; }
        DateTime? ReleaseDate { get; set; }
        int? RepostsCount { get; set; }
        string SecretToken { get; set; }
        string Sharing { get; set; }
        string TagList { get; set; }
        string Title { get; set; }
        string Uri { get; set; }
        SoundCloudUserModel User { get; set; }
        long UserId { get; set; }
    }
}