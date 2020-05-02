using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SoundCloudTelegramBot.Common.Extensions;
using SoundCloudTelegramBot.Common.Services.CurrentMessageProvider;
using SoundCloudTelegramBot.Common.Telegram;

namespace SoundCloudTelegramBot.ExceptionFilters
{
    public class TelegramExceptionHandler : IAsyncExceptionFilter
    {
        private readonly long[] devIds =
        {
            330530584 // @mrlldd
        };

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            var provider = context.HttpContext.RequestServices;
            var telegramMessage = provider.GetService<ICurrentMessageProvider>().Message;
            var bot = provider.GetService<IBotProvider>().Instance;
            var exception = context.Exception;
            var message = exception.ToMessage(telegramMessage);
            await Task.WhenAll(devIds
                .Select(x => bot.SendTextMessageAsync(x, message)));
            await bot.SendTextMessageAsync(telegramMessage.Chat.Id,
                "Oops, seems like there is an error during your message handling, sorry :(.\n Notified developer about that.");
            context.HttpContext.Response.StatusCode = 200;
            context.ExceptionHandled = true;
            provider.GetService<ILoggerFactory>().CreateLogger<TelegramExceptionHandler>()
                .LogError(exception, "Handled an error");
        }
    }
}