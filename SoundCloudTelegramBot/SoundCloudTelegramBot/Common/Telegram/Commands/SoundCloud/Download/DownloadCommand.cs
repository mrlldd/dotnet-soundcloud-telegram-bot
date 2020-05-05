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

            var resultStream = soundCloudInteractor.DownloadTrackAsync(track);
            var thumbnail = DownloadThumbnail(track.ArtworkUrl);
            await Task.WhenAll(resultStream, thumbnail);
            
            await using var audioStream = await resultStream;
            await using var thumbnailStream = new MemoryStream(await thumbnail);
            
            var name = $"{track.User.Username} - {track.Title}";
            await BotProvider.Instance.SendAudioAsync(message.Chat.Id,
                new InputMedia(audioStream, name + ".mp3"),
                $"@{BotProvider.BotInfo.Username}",
                performer: track.User.Username,
                title: track.Title,
                thumb: new InputMedia(thumbnailStream, name + ".jpg"));
        }

        private async Task<byte[]> DownloadThumbnail(string url)
        {
            var client = new RestClient();
            var request = new RestRequest(url, Method.GET);
            var result = await client.ExecuteGetAsync(request);
            return result.RawBytes;
        }
    }
}