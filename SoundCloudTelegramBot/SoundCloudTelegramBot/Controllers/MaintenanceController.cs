using Microsoft.AspNetCore.Mvc;

namespace SoundCloudTelegramBot.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MaintenanceController : Controller
    {
        [HttpGet]
        public bool Status() => true;
    }
}