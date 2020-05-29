using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RestSharp;
using SoundCloudTelegramBot.Common.SoundCloud.Enums;
using SoundCloudTelegramBot.Common.SoundCloud.Interaction;
using SoundCloudTelegramBot.Common.SoundCloud.Models;
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
            var bot = BotProvider.Instance;
            var resolvedEntity = await soundCloudInteractor.ResolveAsync(message.Text);
            if (resolvedEntity == null || resolvedEntity.Kind == EntityKind.User)
            {
                await BotProvider.Instance.SendTextMessageAsync(message.Chat.Id, "Seems like there is wrong url :(");
                return;
            }

            switch (resolvedEntity.Kind)
            {
                case EntityKind.Track:
                {
                    var name = Regex.Replace($"{resolvedEntity.User.Username} - {resolvedEntity.Title}", @"[^\w\s\-]",
                        string.Empty);

                    var resultStream = soundCloudInteractor.DownloadTrackAsync(resolvedEntity as ITrack);
                    var thumbnailTask = resolvedEntity.ArtworkUrl != null
                        ? DownloadThumbnailAsync(resolvedEntity.ArtworkUrl, name)
                        : Task.FromResult<InputMedia>(null);

                    await Task.WhenAll(resultStream, thumbnailTask);

                    await using var audioStream = await resultStream;
                    var thumbnail = await thumbnailTask;
                    await bot.SendAudioAsync(message.Chat.Id,
                        new InputMedia(audioStream, name + ".mp3"),
                        $"@{BotProvider.Info.Username}",
                        performer: resolvedEntity.User.Username,
                        title: resolvedEntity.Title,
                        thumb: thumbnail);
                    return;
                }
                case EntityKind.Playlist:
                {
                    if (!(resolvedEntity is IPlaylist playlist))
                    {
                        throw new InvalidOperationException("Failed to parse playlist.");
                    }

                    var tracks = playlist.Tracks.Take(12).ToArray();
                    var resolved = new List<ITrack>();
                    var hiddenIds = new List<long>();
                    foreach (var track in tracks)
                    {
                        if (string.IsNullOrEmpty(track.Title))
                        {
                            hiddenIds.Add(track.Id);
                            continue;
                        }

                        resolved.Add(track);
                    }

                    var name = Regex.Replace($"{resolvedEntity.User.Username} - {resolvedEntity.Title}", @"[^\w\s\-]",
                        string.Empty);
                    var thumbnailTask = playlist.ArtworkUrl != null
                        ? DownloadThumbnailAsync(resolvedEntity.ArtworkUrl, name)
                        : Task.FromResult<InputMedia>(null);
                    var searchTracksByIdsTask = soundCloudInteractor.SearchTracksByIds(hiddenIds);
                    await Task.WhenAll(thumbnailTask, searchTracksByIdsTask);
                    var searchResult = await searchTracksByIdsTask;
                    var thumbnail = await thumbnailTask;
                    foreach (var track in resolved.Concat(searchResult))
                    {
                        await using var stream = await soundCloudInteractor.DownloadTrackAsync(track);
                        await bot.SendAudioAsync(message.Chat.Id,
                            new InputMedia(stream, name + ".mp3"),
                            $"@{BotProvider.Info.Username}",
                            performer: track.User.Username,
                            title: track.Title,
                            thumb: thumbnail);
                    }

                    break;
                }
                default:
                {
                    throw new InvalidOperationException("Unexpected behaviour.");
                }
            }
        }

        private async Task<InputMedia> DownloadThumbnailAsync(string url, string fileName)
        {
            var client = new RestClient();
            var request = new RestRequest(url, Method.GET);
            var response = await client.ExecuteGetAsync(request);
            return new InputMedia(new MemoryStream(response.RawBytes, false), $"{fileName}.jpg");
        }
    }
}