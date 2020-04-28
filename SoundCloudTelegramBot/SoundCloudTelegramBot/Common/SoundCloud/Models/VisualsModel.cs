namespace SoundCloudTelegramBot.Common.SoundCloud.Models
{
    public class VisualsModel
    {
        public string Urn { get; set; }
        public bool Enabled { get; set; }
        public InnerVisualsModel[] Visuals { get; set; }
        public string Tracking { get; set; }
    }
}