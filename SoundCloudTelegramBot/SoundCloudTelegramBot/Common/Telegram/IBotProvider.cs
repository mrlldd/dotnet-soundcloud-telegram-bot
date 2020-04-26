using System.Threading.Tasks;
using Telegram.Bot;

namespace SoundCloudTelegramBot.Common.Telegram
{
    public interface IBotProvider
    {
        public Task Initialize(string webhookUrl);
        public ITelegramBotClient Instance { get; }
    }
}