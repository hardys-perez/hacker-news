using HackerNews.HttpClients;
using HackerNews.Server;
using HackerNews.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HackerNews
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddHttpClient<IHackerNewsService, HackerNewsService>(
                client =>
                {
                    client.BaseAddress = new System.Uri(Configuration["HackerNewsUrl"]);
                });

            services.AddSignalR();
            services.AddSingleton<IConsumerService, ConsumerService>();
            services.AddSingleton<IMessageSender, MessageSender>();

            services.AddHostedService<StoryPullerService>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseSignalR(routes =>
            {
                routes.MapHub<TopStoriesHub>("/tophackernews");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=TopStories}/{action=Index}");
            });

        }
    }
}
