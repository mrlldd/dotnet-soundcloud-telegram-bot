using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SoundCloudTelegramBot.ActionFilters;
using SoundCloudTelegramBot.Common.Extensions;
using SoundCloudTelegramBot.Common.Telegram;
using SoundCloudTelegramBot.Common.Telegram.Commands;
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
        private readonly IDispatcher dispatcher;

        public TelegramController(ILogger<TelegramController> logger, IBotProvider botProvider,
            IDispatcher dispatcher)
        {
            this.logger = logger;
            this.botProvider = botProvider;
            this.dispatcher = dispatcher;
        }

        [HttpPost]
        public Task Update([FromBody] Update update)
        {
            logger.LogTelegramMessage(update);
            var bot = botProvider.Instance;
            if (update.Message.IsCommand())
            {
                return dispatcher.DispatchCommandAsync(update.Message);
            }
            //throw new InvalidOperationException("Hey, this is an error!");
            return bot.SendTextMessageAsync(update.Message.Chat.Id, "Unknown message, sorry :(");
        }
    }
}