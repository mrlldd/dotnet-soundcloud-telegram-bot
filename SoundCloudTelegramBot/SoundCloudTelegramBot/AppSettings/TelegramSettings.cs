using Telegram.Bot;

namespace SoundCloudTelegramBot.AppSettings
{
    public class TelegramSettings : ITelegramSettings
    {
        public string BotToken { get; set; }
    }
}