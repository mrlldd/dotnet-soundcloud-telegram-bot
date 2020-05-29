using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SoundCloudTelegramBot.Common.Extensions;
using SoundCloudTelegramBot.Common.Services.CurrentMessageProvider;
using SoundCloudTelegramBot.Common.Telegram;
using Telegram.Bot.Types.Enums;

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
            var updateProvider = provider.GetService<ICurrentMessageProvider>();
            var bot = provider.GetService<IBotProvider>().Instance;
            var exception = context.Exception;
            string message;
            switch (updateProvider.UpdateType)
            {
                case UpdateType.Message:
                {
                    message = exception.ToMessage(updateProvider.Message);
                    break;
                }
                case UpdateType.CallbackQuery:
                {
                    message = exception.ToMessage(updateProvider.CallbackQuery);
                    break;
                }
                default:
                {
                    message = exception.ToMessage() + "\n Source: Unknown";
                    break;
                }
            }

            await Task.WhenAll(devIds
                .Select(x => bot.SendTextMessageAsync(x, message))
                .Append(bot.SendTextMessageAsync(updateProvider.Chat.Id,
                    "Oops, seems like there is an error during your message handling, sorry :(\n" +
                    "Notified developer about that."))
            );
            context.HttpContext.Response.StatusCode = 200;
            context.ExceptionHandled = true;
            provider.GetService<ILoggerFactory>().CreateLogger<TelegramExceptionHandler>()
                .LogError(exception, "Handled an error");
        }
    }
}