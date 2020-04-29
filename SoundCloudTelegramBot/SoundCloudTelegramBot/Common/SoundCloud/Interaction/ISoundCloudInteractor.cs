using System.IO;
using System.Threading.Tasks;
using SoundCloudTelegramBot.Common.SoundCloud.Models;

namespace SoundCloudTelegramBot.Common.SoundCloud.Interaction
{
    public interface ISoundCloudInteractor
    {
        Task<SearchTracksResultModel> SearchTracks(string query);
        Task<Stream> DownloadTrackAsync(TrackModel trackModel);
        Task<TrackModel> ResolveTrackAsync(string trackUrl);
    }
}