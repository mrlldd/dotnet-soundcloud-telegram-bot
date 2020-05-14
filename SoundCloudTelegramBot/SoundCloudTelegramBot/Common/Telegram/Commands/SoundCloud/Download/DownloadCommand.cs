using System.IO;
using System.Threading.Tasks;
using RestSharp;
using SoundCloudTelegramBot.Common.SoundCloud.Interaction;
using Telegram.Bot.Types;

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
            if (track == null)
            {
                await BotProvider.Instance.SendTextMessageAsync(message.Chat.Id, "Seems like there is wrong url :(");
                return;
            }

            var name = $"{track.User.Username} - {track.Title}";

            var resultStream = soundCloudInteractor.DownloadTrackAsync(track);
            var thumbnailTask = track.ArtworkUrl != null
                ? DownloadThumbnail(track.ArtworkUrl, name)
                : Task.FromResult<InputMedia>(null);

            await Task.WhenAll(resultStream, thumbnailTask);

            await using var audioStream = await resultStream;
            var thumbnail = await thumbnailTask;
            await BotProvider.Instance.SendAudioAsync(message.Chat.Id,
                new InputMedia(audioStream, name + ".mp3"),
                $"@{BotProvider.BotInfo.Username}",
                performer: track.User.Username,
                title: track.Title,
                thumb: thumbnail);
        }

        private async Task<InputMedia> DownloadThumbnail(string url, string fileName)
        {
            var client = new RestClient();
            var request = new RestRequest(url, Method.GET);
            var response = await client.ExecuteGetAsync(request);
            return new InputMedia(new MemoryStream(response.RawBytes, false), $"{fileName}.jpg");
        }
    }
}