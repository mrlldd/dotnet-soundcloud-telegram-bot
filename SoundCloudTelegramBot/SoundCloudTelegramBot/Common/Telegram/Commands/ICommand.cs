﻿using System.Threading.Tasks;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;

namespace SoundCloudTelegramBot.Common.Telegram.Commands
{
    public interface ICommand
    {
        string Name { get; }
        Task ExecuteAsync(Message message);
    }
}