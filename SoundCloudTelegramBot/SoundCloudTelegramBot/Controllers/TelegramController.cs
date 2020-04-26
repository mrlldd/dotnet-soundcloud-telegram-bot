
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace SoundCloudTelegramBot.Controllers
{
    [ApiController]
    [Route("/api/telegram/[action]")]
    public class TelegramController : ControllerBase
    {
        public TelegramController()
        {
            
        }


        [HttpPost]
        public void Update([FromBody] Update update)
        {
            
        }
    }
}