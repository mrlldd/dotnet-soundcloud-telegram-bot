using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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

namespace SoundCloudTelegramBot
{
    public class Startup
    {
        private readonly IConfiguration configurationRoot;
        private AppConfiguration appConfiguration;
        private IServiceCollection serviceCollection;
        public Startup(IConfiguration configurationRoot)
        {
            this.configurationRoot = configurationRoot;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            serviceCollection = services;
            services.AddControllers().AddNewtonsoftJson();
            //services.AddSingleton<IAppConfiguration>(appConfiguration);
            services.AddLogging();
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
                appConfiguration = new AppConfiguration(new TelegramSettings(), new SoundCloudSettings());
                configurationRoot.Bind(appConfiguration);
            }
            else if (env.IsProduction())
            {
                var logger = app.ApplicationServices.GetService<ILoggerFactory>().CreateLogger<Startup>();
                var envDictionary = new Dictionary<string, string>();
                foreach (var item in Environment.GetEnvironmentVariables())
                {
                    var entry = item is DictionaryEntry dictionaryEntry ? dictionaryEntry : default;
                    envDictionary[entry.Key.ToString()] = entry.Value.ToString();
                    logger.LogInformation($"{entry.Key} - {entry.Value}");
                }

                var soundCloudSettings = new SoundCloudSettings
                {
                    ClientId = envDictionary["CLIENTID"],
                    OAuthToken = envDictionary["OAUTHTOKEN"]
                };
                var telegramSettings = new TelegramSettings
                {
                    BotToken = envDictionary["BOTTOKEN"]
                };
                var appConfig = new AppConfiguration(telegramSettings, soundCloudSettings);
                serviceCollection.AddSingleton<IAppConfiguration, AppConfiguration>(_ => appConfig);
            }

            app.UseMiddleware<ResponseTimeMiddleware>();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", context => context.Response.WriteAsync("This is my new telega bot!"));
                endpoints.MapControllers();
            });
            app.UseHttpsRedirection();
        }
    }
}