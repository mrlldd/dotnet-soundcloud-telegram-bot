using System.Threading.Tasks;
using SoundCloudTelegramBot.Common.Caches.Search;
using SoundCloudTelegramBot.Common.Extensions;
using SoundCloudTelegramBot.Common.SoundCloud.Interaction;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace SoundCloudTelegramBot.Common.Telegram.Commands.SoundCloud.Resolve
{
    public class ResolveCommand : CommandBase, IResolveCommand
    {
        private readonly ISearchCache searchCache;
        private readonly ISoundCloudInteractor soundCloudInteractor;

        public ResolveCommand(IBotProvider botProvider, ISearchCache searchCache,
            ISoundCloudInteractor soundCloudInteractor) : base(botProvider)
        {
            this.searchCache = searchCache;
            this.soundCloudInteractor = soundCloudInteractor;
        }

        public override string Name { get; } = "resolve";

        public override async Task ExecuteAsync(Message message)
        {
            ResolveResult result = null;
            if (int.TryParse(message.Text, out var id) &&
                searchCache.TryGetTrackUrl(message.Chat.Id, id, out var cachedTrack))
            {
                result = new ResolveResult
                {
                    Author = cachedTrack.Author,
                    AvatarUrl = cachedTrack.ImageUrl,
                    Name = cachedTrack.Name,
                    Uri = cachedTrack.Uri
                };
            }
            else if (message.Text.TryExtractSoundCloudUrl(out var url))
            {
                var track = await soundCloudInteractor.ResolveTrackAsync(url);
                if (track != null)
                {
                    result = new ResolveResult
                    {
                        Author = track.User.Username,
                        Name = track.Title,
                        Uri = track.Uri,
                        AvatarUrl = track.ArtworkUrl ?? track.User.AvatarUrl
                    };
                }
            }

            if (result == null)
            {
                await BotProvider.Instance.SendTextMessageAsync(message.Chat.Id, "Your message resolving failed :(");
                return;
            }
            var keyboard = new InlineKeyboardMarkup(new InlineKeyboardButton
            {
                Text = "Download",
                CallbackData = "/download " + result.Uri,
            });
            await BotProvider.Instance.SendPhotoAsync(message.Chat.Id,
                new InputOnlineFile(result.AvatarUrl.Replace("large", "t300x300")),
                $"{result.Author} - {result.Name}",
                replyMarkup: keyboard);
        }

        private class ResolveResult
        {
            public string AvatarUrl { get; set; }
            public string Author { get; set; }
            public string Name { get; set; }
            public string Uri { get; set; }
        }
    }
}