using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace SoundCloudTelegramBot.Middleware
{
    public class ResponseTimeMiddleware
    {
        private readonly RequestDelegate next;

        public ResponseTimeMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context, ILogger<ResponseTimeMiddleware> logger)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            await next(context);
            stopwatch.Stop();
            logger.LogInformation("Response time: " + stopwatch.Elapsed);
        }
    }
}