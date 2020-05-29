using System.Linq;

namespace SoundCloudTelegramBot.Common.SoundCloud.Models.Search
{
    public class SearchResult<TEntity> : ISearchResult<TEntity> where TEntity : ITypedEntity
    {
        public TEntity[] Collection { get; set; }
        public string NextHref { get; set; }
        public string QueryUrn { get; set; }
        public int TotalResults { get; set; }

        public ISearchResult<ITypedEntity> ToAbstractLevelEntity() =>
            new SearchResult<ITypedEntity>
            {
                Collection = Collection
                    .OfType<ITypedEntity>()
                    .ToArray(),
                NextHref = NextHref,
                QueryUrn = QueryUrn,
                TotalResults = TotalResults
            };
    }
}