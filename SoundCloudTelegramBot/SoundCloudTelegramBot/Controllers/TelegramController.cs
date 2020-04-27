using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SoundCloudTelegramBot.ActionFilters;
using SoundCloudTelegramBot.Common.Extensions;
using SoundCloudTelegramBot.Common.Telegram;
using SoundCloudTelegramBot.Common.Telegram.Commands.SoundCloud.Search;
using Telegram.Bot.Types;
using SoundCloudTelegramBot.ExceptionFilters;

namespace SoundCloudTelegramBot.Controllers
{
    [ApiController]
    [WrapCurrentMessage]
    [TypeFilter(typeof(TelegramExceptionHandler))]
    [Route("api/[controller]/[action]")]
    public class TelegramController : ControllerBase
    {
        private readonly ILogger<TelegramController> logger;
        private readonly IBotProvider botProvider;
        private readonly ISearchTracksCommand searchTracksCommand;

        public TelegramController(ILogger<TelegramController> logger, IBotProvider botProvider,
            ISearchTracksCommand searchTracksCommand)
        {
            this.logger = logger;
            this.botProvider = botProvider;
            this.searchTracksCommand = searchTracksCommand;
        }

        [HttpPost]
        public Task Update([FromBody] Update update)
        {
            logger.LogTelegramMessage(update);
            var bot = botProvider.Instance;
            if (update.Message.IsCommand())
            {
                searchTracksCommand.ExecuteAsync();
            }
            //throw new InvalidOperationException("Hey, this is an error!");
            return bot.SendTextMessageAsync(update.Message.Chat.Id, "Unknown message, sorry :(");
        }
    }
}