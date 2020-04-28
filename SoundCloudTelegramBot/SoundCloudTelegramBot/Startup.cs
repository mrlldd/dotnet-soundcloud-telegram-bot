using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SoundCloudTelegramBot.AppSettings;
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
        private readonly AppConfiguration appConfiguration;

        public Startup(IConfiguration configurationRoot)
        {
            this.configurationRoot = configurationRoot;
            appConfiguration = new AppConfiguration();
            configurationRoot.Bind(appConfiguration);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();
            services.AddSingleton<IAppConfiguration>(appConfiguration);
            services.AddLogging();
            services.AddSingleton<IBotProvider, BotProvider>();
            services.AddHostedService<BotInitializerHostedService>();
            services.AddScoped<ICurrentMessageProvider, CurrentMessageProvider>();
            services.AddHttpClient();
            services.AddTelegramCommands();
            services.AddSoundCloudServices();
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
                endpoints.MapGet("/", context => context.Response.WriteAsync("This is my new telega bot!"));
                endpoints.MapControllers();
            });
            app.UseHttpsRedirection();
        }
    }
}