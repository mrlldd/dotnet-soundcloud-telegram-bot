using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoundCloudTelegramBot.Common.SoundCloud.Interaction;
using SoundCloudTelegramBot.Common.SoundCloud.Models;
using Telegram.Bot.Types;

namespace SoundCloudTelegramBot.Common.Telegram.Commands.SoundCloud.Search
{
    public class SearchTracksCommand : CommandBase, ISearchTracksCommand
    {
        private readonly ISoundCloudInteractor soundCloudInteractor;
        public override string Name { get; } = "search";

        public SearchTracksCommand(IBotProvider botProvider, ISoundCloudInteractor soundCloudInteractor)
            : base(botProvider)
        {
            this.soundCloudInteractor = soundCloudInteractor;
        }

        public override async Task ExecuteAsync(Message message)
        {
            var result = await soundCloudInteractor.SearchTracks(message.Text);
            await BotProvider.Instance.SendTextMessageAsync(message.Chat.Id,
                BuildResponse(result.Collection));
        }

        private static string BuildResponse(IEnumerable<TrackModel> tracks)
            => tracks
                .Select((x, index) => $"/{index + 1} {x.User.Username} - {x.Title}")
                .Aggregate((prev, next) => prev + "\n" + next);
    }
}