using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SoundCloudTelegramBot.Common.Telegram
{
    public interface IBotProvider
    {
        public Task InitializeAsync(string webhookUrl);
        public ITelegramBotClient Instance { get; }
        public User Info { get; }
    }
}