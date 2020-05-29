using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using SoundCloudTelegramBot.Common.Caches.Search;
using SoundCloudTelegramBot.Common.SoundCloud.Enums;
using SoundCloudTelegramBot.Common.SoundCloud.Interaction;
using SoundCloudTelegramBot.Common.SoundCloud.Models;
using Telegram.Bot.Types;

namespace SoundCloudTelegramBot.Common.Telegram.Commands.SoundCloud.Search
{
    public class SearchTracksCommand : CommandBase, ISearchTracksCommand
    {
        private readonly ISoundCloudInteractor soundCloudInteractor;
        private readonly ISearchCache searchCache;
        public override string Name { get; } = "search";

        public SearchTracksCommand(IBotProvider botProvider,
            ISoundCloudInteractor soundCloudInteractor,
            ISearchCache searchCache)
            : base(botProvider)
        {
            this.soundCloudInteractor = soundCloudInteractor;
            this.searchCache = searchCache;
        }

        public override async Task ExecuteAsync(Message message)
        {
            var result = await soundCloudInteractor.SearchAsync(message.Text);
            var collection = result.Collection
                .Where(x => x.Kind != EntityKind.User)
                .ToArray();
            if (collection.Length == 0)
            {
                await BotProvider.Instance.SendTextMessageAsync(message.Chat.Id, "There is no such tracks :(");
                return;
            }
            searchCache.Set(message.Chat.Id, collection);
            await BotProvider.Instance.SendTextMessageAsync(message.Chat.Id,
                BuildResponse(collection));
        }
        
        private static string BuildResponse(IEnumerable<ITypedEntity> entities)
            => entities
                .Select((x, index) => $"/{index + 1} {ResolveType(x.Kind)} {x.User.Username} - {x.Title} - " + TimeSpan.FromMilliseconds(x.Duration).ToString("mm\\:ss"))
                .Aggregate((prev, next) => prev + "\n" + next);

        private static string ResolveType(EntityKind kind)
        {
            switch (kind)
            {
                case EntityKind.Track:
                {
                    return "🎵";
                }
                case EntityKind.Playlist:
                {
                    return "📀";
                }
                default:
                {
                    return "❌";
                }
            }
        }
    }
}