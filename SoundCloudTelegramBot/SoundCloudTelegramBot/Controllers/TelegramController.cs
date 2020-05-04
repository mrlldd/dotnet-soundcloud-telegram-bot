﻿using System;
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
    [Route("/api/[controller]/[action]")]
    public class TelegramController : ControllerBase
    {
        private readonly ILogger<TelegramController> logger;
        private readonly IDispatcher dispatcher;

        public TelegramController(ILogger<TelegramController> logger, IDispatcher dispatcher)
        {
            this.logger = logger;
            this.dispatcher = dispatcher;
        }

        [HttpPost]
        public Task Update([FromBody] Update update)
        {
            logger.LogTelegramMessage(update);
            if (update.Message.IsCommand())
            {
                return dispatcher.DispatchCommandAsync(update.Message);
            }

            update.Message.Text = update.Message.Text.Trim().Insert(0, "/search ");
            return dispatcher.DispatchCommandAsync(update.Message);
        }
    }
}