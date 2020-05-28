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
using Telegram.Bot.Types.Enums;

namespace SoundCloudTelegramBot.Controllers
{
    [ApiController]
    [WrapCurrentMessage]
    [TypeFilter(typeof(TelegramExceptionHandler))]
    [Route("/api/telegram")]
    public class TelegramController : ControllerBase
    {
        private readonly IDispatcher dispatcher;

        public TelegramController(IDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        [HttpPost("update")]
        public Task Update([FromBody] Update update) 
            => dispatcher.DispatchAsync(update);

        /*{
            logger.LogTelegramMessage(update);
            Message message;
            if (update.Type == UpdateType.CallbackQuery)
            {
                message = update.CallbackQuery.Message;
                message.Text = update.CallbackQuery.Data;
            }
            else
            {
                message = update.Message;
            }
            message.Text = message.Text.Trim();
            if (message.Text.IsCommand())
            {
                return dispatcher.DispatchAsync(update);
            }
            update.Message.Text = update.Message.Text.TryExtractSoundCloudUrl(out var url)
                ? $"/resolve {url}"
                : $"/search {message.Text}";
            return dispatcher.DispatchAsync(update);
        }*/
    }
}