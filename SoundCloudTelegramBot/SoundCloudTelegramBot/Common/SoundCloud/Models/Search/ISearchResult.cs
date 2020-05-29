using System.Collections.Generic;

namespace SoundCloudTelegramBot.Common.SoundCloud.Models.Search
{
    public interface ISearchResult<TEntity> where TEntity : ITypedEntity
    {
        TEntity[] Collection { get; set; }
        string NextHref { get; set; }
        string QueryUrn { get; set; }
        int TotalResults { get; set; }
    }
}