﻿using SoundCloudTelegramBot.Common.Caches.Search.Models;
using SoundCloudTelegramBot.Common.SoundCloud.Models;

namespace SoundCloudTelegramBot.Common.Caches.Search
{
    public interface ISearchCache
    {
        void Set(long chatId, SearchTracksResultModel model);
        bool TryGetTrackUrl(long chatId, int id, out CachedTrack trackUrl);
    }
}