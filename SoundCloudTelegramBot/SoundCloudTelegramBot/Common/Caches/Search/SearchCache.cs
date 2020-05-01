using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using SoundCloudTelegramBot.Common.SoundCloud.Models;

namespace SoundCloudTelegramBot.Common.Caches.Search
{
    public class SearchCache : ISearchCache
    {
        private readonly IMemoryCache memoryCache;
        private readonly ILogger<SearchCache> logger;

        public SearchCache(IMemoryCache memoryCache, ILogger<SearchCache> logger)
        {
            this.memoryCache = memoryCache;
            this.logger = logger;
        }

        public void Set(long chatId, SearchTracksResultModel model)
        {
            var collection = model.Collection;
            var entryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(1)
            };
            for (var index = 0; index < collection.Length; index++)
            {
                var key = $"{index + 1}st{chatId}";
                var uri = collection[index].Uri;
                memoryCache.Set(key, uri, entryOptions);
            }

            logger.LogInformation($"Successfully cached {collection.Length} tracks for chat {chatId}");
        }

        public bool TryGetTrackUrl(long chatId, int id, out string trackUrl)
            => memoryCache.TryGetValue($"{id}st{chatId}", out trackUrl);
    }
}