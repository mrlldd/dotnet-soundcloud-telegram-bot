using System;

namespace SoundCloudTelegramBot.Common.Caches.Search.Models
{
    public class CachedTrack
    {
        public string Uri { get; set; }
        public string Author { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public TimeSpan Duration { get; set; }
    }
}