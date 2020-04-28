namespace SoundCloudTelegramBot.Common.SoundCloud.Models
{
    public class SearchTracksResultModel
    {
        public TrackModel[] Collection { get; set; }
        public string NextHref { get; set; }
        public string QueryUrn { get; set; }
        public int TotalResults { get; set; }
    }
}