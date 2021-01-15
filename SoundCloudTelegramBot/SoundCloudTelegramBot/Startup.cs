using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SoundCloudTelegramBot.AppSettings;
using SoundCloudTelegramBot.AppSettings.SoundCloud;
using SoundCloudTelegramBot.AppSettings.Telegram;
using SoundCloudTelegramBot.Common.Caches;
using SoundCloudTelegramBot.Common.HostedServices;
using SoundCloudTelegramBot.Common.Services.CurrentMessageProvider;
using SoundCloudTelegramBot.Common.SoundCloud;
using SoundCloudTelegramBot.Common.Telegram;
using SoundCloudTelegramBot.Common.Telegram.Commands;
using SoundCloudTelegramBot.Middleware;
using Telegram.Bot.Types.Enums;

namespace SoundCloudTelegramBot
{
    public class Startup
    {
        private readonly IConfiguration configurationRoot;
        private readonly IHostEnvironment environment;
        private AppConfiguration appConfiguration;

        public Startup(IConfiguration configurationRoot,
            IHostEnvironment environment)
        {
            this.configurationRoot = configurationRoot;
            this.environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            if (environment.IsDevelopment())
            {
                appConfiguration = new AppConfiguration(new TelegramSettings(), new SoundCloudSettings());
                configurationRoot.Bind(appConfiguration);
                services.AddSingleton<IAppConfiguration>(appConfiguration);
            }
            else if (environment.IsProduction())
            {
                var envDictionary = new Dictionary<string, string>();
                foreach (var item in Environment.GetEnvironmentVariables())
                {
                    var entry = item is DictionaryEntry dictionaryEntry ? dictionaryEntry : default;
                    envDictionary[entry.Key.ToString()] = entry.Value.ToString();
                }
            
                var soundCloudSettings = new SoundCloudSettings
                {
                    ClientId = envDictionary[nameof(SoundCloudSettings.ClientId).ToUpper()]
                };
                var telegramSettings = new TelegramSettings
                {
                    BotToken = envDictionary[nameof(TelegramSettings.BotToken).ToUpper()]
                };
                var appConfig = new AppConfiguration(telegramSettings, soundCloudSettings)
                {
                    WebhookUrl = "https://soundcloud-in-play-tg-bot.herokuapp.com",
                    AllowedUpdates = configurationRoot.GetSection(nameof(AppConfiguration.AllowedUpdates)).Get<UpdateType[]>()
                };
                services.AddSingleton<IAppConfiguration>(appConfig);
            }

            services.AddControllers().AddNewtonsoftJson();
            //services.AddSingleton<IAppConfiguration>(appConfiguration);
            services.AddSingleton<IBotProvider, BotProvider>();
            services.AddHostedService<BotInitializerHostedService>();
            services.AddScoped<ICurrentMessageProvider, CurrentMessageProvider>();
            services.AddHttpClient();
            services.AddTelegramCommands();
            services.AddSoundCloudServices();
            services.AddCaches();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ResponseTimeMiddleware>();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", context =>
                {
#if DEBUG
                    return context.Response.WriteAsync("This is my new telega bot!");
#else
                    context.Response.Redirect("https://t.me/soundcloudinplaybot");
                    return Task.CompletedTask;
#endif
                });
                endpoints.MapControllers();
            });
            app.UseHttpsRedirection();
        }
    }
}