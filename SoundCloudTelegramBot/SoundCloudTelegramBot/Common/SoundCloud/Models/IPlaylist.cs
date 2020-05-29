using System;

namespace SoundCloudTelegramBot.Common.SoundCloud.Models
{
    public interface IPlaylist : ITypedEntity
    {
        bool IsAlbum { get; set; }
        bool ManagedByFeeds { get; set; }
        DateTime PublishedAt { get; set; }
        string SetType { get; set; }
        int TrackCount { get; set; }
        Track[] Tracks { get; set; }
    }
}