﻿using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SoundCloudTelegramBot.Common.Telegram
{
    public interface IBotProvider
    {
        public Task InitializeAsync(Func<Task<ITelegramBotClient>> provideBotFunction);
        public ITelegramBotClient Instance { get; }
        public User Info { get; }
    }
}