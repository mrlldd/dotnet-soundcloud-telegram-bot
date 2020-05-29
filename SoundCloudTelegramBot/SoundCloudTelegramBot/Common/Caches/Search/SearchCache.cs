using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using SoundCloudTelegramBot.Common.Caches.Search.Models;
using SoundCloudTelegramBot.Common.SoundCloud.Models;

namespace SoundCloudTelegramBot.Common.Caches.Search
{
    public class SearchCache : ISearchCache
    {
        private readonly IMemoryCache memoryCache;
        private readonly ILogger<SearchCache> logger;
        private const string keyFormat = "{0}st{1}";

        public SearchCache(IMemoryCache memoryCache, ILogger<SearchCache> logger)
        {
            this.memoryCache = memoryCache;
            this.logger = logger;
        }

        public void Set(long chatId, ITypedEntity[] collection)
        {
            var entryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(1)
            };
            foreach (var entry in collection
                .Select((x, index) => new
                {
                    Index = index + 1,
                    Track = new CachedTrack
                    {
                        Author = x.User.Username,
                        ImageUrl = x.ArtworkUrl ?? x.User.AvatarUrl,
                        Name = x.Title,
                        Uri = x.Uri
                    }
                }))
            {
                memoryCache.Set(string.Format(keyFormat, entry.Index, chatId), entry.Track, entryOptions);
            }

            logger.LogInformation($"Successfully cached {collection.Length} tracks for chat {chatId}");
        }
        // todo all cache update on any get
        public bool TryGetTrackUrl(long chatId, int id, out CachedTrack trackUrl)
            => memoryCache.TryGetValue(string.Format(keyFormat, id, chatId), out trackUrl);
    }
}