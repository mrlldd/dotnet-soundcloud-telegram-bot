
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Telegram.Bot.Types;

namespace SoundCloudTelegramBot.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TelegramController : ControllerBase
    {
        private readonly ILogger<TelegramController> logger;

        public TelegramController(ILogger<TelegramController> logger)
        {
            this.logger = logger;
        }


        [HttpPost]
        public Task Update([FromBody] Update update)
        {
            logger.LogInformation(JsonConvert.SerializeObject(update, Formatting.Indented));
        }
    }
}