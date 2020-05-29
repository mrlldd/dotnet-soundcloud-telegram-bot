using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SoundCloudTelegramBot.Common.SoundCloud.Models;
using SoundCloudTelegramBot.Common.SoundCloud.Models.Search;

namespace SoundCloudTelegramBot.Common.SoundCloud.Interaction
{
    public interface ISoundCloudInteractor
    {
        Task<ISearchResult<ITypedEntity>> SearchAsync(string query);
        Task<Stream> DownloadTrackAsync(ITrack track);
        Task<ITypedEntity> ResolveAsync(string trackUrl);
        Task<ITrack[]> SearchTracksByIds(IEnumerable<long> ids);
    }
}