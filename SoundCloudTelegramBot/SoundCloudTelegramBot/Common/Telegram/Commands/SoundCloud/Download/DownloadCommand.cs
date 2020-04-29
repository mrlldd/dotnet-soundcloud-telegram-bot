using System.Threading.Tasks;
using SoundCloudTelegramBot.Common.SoundCloud.Interaction;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace SoundCloudTelegramBot.Common.Telegram.Commands.SoundCloud.Download
{
    public class DownloadCommand : CommandBase, IDownloadCommand
    {
        private readonly ISoundCloudInteractor soundCloudInteractor;
        public override string Name { get; } = "download";

        public DownloadCommand(IBotProvider botProvider, ISoundCloudInteractor soundCloudInteractor) : base(botProvider)
        {
            this.soundCloudInteractor = soundCloudInteractor;
        }

        public override async Task ExecuteAsync(Message message)
        {
            var track = await soundCloudInteractor.ResolveTrackAsync(message.Text);
            var resultStream = await soundCloudInteractor.DownloadTrackAsync(track);
            //BotProvider.Instance.SendAudioAsync(message.Chat.Id)
            await BotProvider.Instance.SendTextMessageAsync(message.Chat.Id, "hey you triggered download command!");
            await BotProvider.Instance.SendAudioAsync(message.Chat.Id, new InputMedia(resultStream, $"{track.User.Username} - {track.Title}"));
        }
    }
}