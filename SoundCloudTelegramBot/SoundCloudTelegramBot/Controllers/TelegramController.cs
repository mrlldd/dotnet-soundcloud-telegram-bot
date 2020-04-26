
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SoundCloudTelegramBot.Common.Extensions;
using SoundCloudTelegramBot.Common.Telegram;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SoundCloudTelegramBot.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TelegramController : ControllerBase
    {
        private readonly ILogger<TelegramController> logger;
        private readonly IBotProvider botProvider;

        public TelegramController(ILogger<TelegramController> logger, IBotProvider botProvider)
        {
            this.logger = logger;
            this.botProvider = botProvider;
        }

        [HttpPost]
        public Task Update([FromBody] Update update)
        {
            logger.LogTelegramMessage(update);
            var bot = botProvider.Instance;
            logger.LogInformation(update.Type.ToString());
            return bot.SendTextMessageAsync(update.Message.Chat.Id, "Unknown message, sorry :(");
        }
    }
}