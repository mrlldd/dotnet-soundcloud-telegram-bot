namespace SoundCloudTelegramBot.Common.SoundCloud.Models
{
    public class TranscodingModel
    {
        public int Duration { get; set; }
        public FormatModel Format { get; set; }
        public string Preset { get; set; }
        public string Quality { get; set; }
        public bool Snipped { get;set; }
        public string Url { get; set; }
    }
}