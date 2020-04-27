using System.Threading.Tasks;
using SoundCloudTelegramBot.Common.Services.CurrentMessageProvider;

namespace SoundCloudTelegramBot.Common.Telegram.Commands.SoundCloud.Search
{
    public class SearchTracksCommand : CommandBase, ISearchTracksCommand
    {
        public override string Name { get; } = "search";

        public SearchTracksCommand(IBotProvider botProvider, ICurrentMessageProvider currentMessageProvider)
            : base(botProvider, currentMessageProvider)
        {
        }

        public override Task ExecuteAsync()
        {
            var message = MessageProvider.Message;
            return BotProvider.Instance.SendTextMessageAsync(message.Chat.Id,
                "wow you triggered search command! " + message.Text.Replace($"/{Name}", string.Empty).Trim());
        }
    }
}