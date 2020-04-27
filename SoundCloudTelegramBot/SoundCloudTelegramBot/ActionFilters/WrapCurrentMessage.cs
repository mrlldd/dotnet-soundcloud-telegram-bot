using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using SoundCloudTelegramBot.Common.Services.CurrentMessageProvider;
using Telegram.Bot.Types;

namespace SoundCloudTelegramBot.ActionFilters
{
    public class WrapCurrentMessageAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var messageProvider = context.HttpContext.RequestServices.GetService<ICurrentMessageProvider>();
            if (!context.ActionArguments.TryGetValue("update", out var update))
            {
                throw new InvalidOperationException("There is no message.");
            }
            
            messageProvider.Set(((Update)update).Message);
        }
    }
}