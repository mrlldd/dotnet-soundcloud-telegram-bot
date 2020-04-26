using System.Threading.Tasks;

namespace SoundCloudTelegramBot.Common.Telegram
{
    public interface IBotProvider
    {
        public Task Initialize(string webhook);
    }
}