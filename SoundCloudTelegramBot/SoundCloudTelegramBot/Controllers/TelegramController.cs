using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SoundCloudTelegramBot.ActionFilters;
using SoundCloudTelegramBot.Common.Caches.Search;
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
    [Route("/")]
    public class TelegramController : ControllerBase
    {
        private readonly ILogger<TelegramController> logger;
        private readonly IBotProvider botProvider;
        private readonly IDispatcher dispatcher;
        private readonly ISearchCache searchCache;

        public TelegramController(ILogger<TelegramController> logger, IBotProvider botProvider,
            IDispatcher dispatcher, ISearchCache searchCache)
        {
            this.logger = logger;
            this.botProvider = botProvider;
            this.dispatcher = dispatcher;
            this.searchCache = searchCache;
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
            //return bot.SendTextMessageAsync(update.Message.Chat.Id, "Unknown message, sorry :(");
            update.Message.Text = update.Message.Text.Trim().Insert(0, "/search ");
            return dispatcher.DispatchCommandAsync(update.Message);
        }

        [HttpGet]
        public string Index() => "Go away please man";
    }
}