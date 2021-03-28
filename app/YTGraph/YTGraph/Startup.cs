using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using YTGraph.Data.Neo;
using YTGraph.Data.Neo.Configuration;
using YTGraph.Selenium.DynamicPageHelper;
using YTGraph.Selenium.DynamicPageHelper.Configuration;
using YTGraph.Selenium.DynamicPageHelper.Contracts;
using YTGraph.Services;

namespace YTGraph
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<NeoConnection>();
            services.AddTransient<IDriverProvider, SeleniumDriverProvider>();
            services.Configure<NeoConfiguration>(Configuration.GetSection("Neo"));
            services.Configure<SeleniumConfiguration>(Configuration.GetSection("Selenium"));

            services.AddMediatR(typeof(MediatorAnchor));
            services.AddRazorPages();
            services.AddServerSideBlazor();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}