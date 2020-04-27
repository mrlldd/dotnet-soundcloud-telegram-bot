using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SoundCloudTelegramBot.Common.Services.CurrentMessageProvider;
using SoundCloudTelegramBot.Common.Telegram;

namespace SoundCloudTelegramBot.ExceptionFilters
{
    public class TelegramExceptionHandler : IAsyncExceptionFilter
    {
        public async Task OnExceptionAsync(ExceptionContext context)
        {
            var provider = context.HttpContext.RequestServices;
            var messageProvider = provider.GetService<ICurrentMessageProvider>();
            var bot = provider.GetService<IBotProvider>().Instance;
            var exception = context.Exception;
            await bot.SendTextMessageAsync(messageProvider.Message.Chat.Id,
                "Oops, seems like there is an error during your message \"" + messageProvider.Message.Text + "\":\n\"" + exception.Message +
                "\"\nSorry :( \nProbably developer already knows about that.");
            context.HttpContext.Response.StatusCode = 200;
            context.ExceptionHandled = true;
            provider.GetService<ILoggerFactory>().CreateLogger<TelegramExceptionHandler>().LogError(exception, "Handled an error");
        }
    }
}