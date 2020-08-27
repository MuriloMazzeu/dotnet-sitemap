using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SitemapCore;
using SitemapCore.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI
{
    public class MyCustomSitemapProvider : ISitemapProvider
    {
        public MyCustomSitemapProvider(ILocationHelper locationHelper)
        {
            _locationHelper = locationHelper;
        }

        private readonly ILocationHelper _locationHelper;

        public Task<IEnumerable<Sitemap>> InvokeAsync()
        {
            var location = _locationHelper.GetSitemapUrl("example-path");
            var myMock = new Sitemap(location)
            {
                ChangeFrequency = ChangeFrequency.Monthly,
                Priority = SitemapPriority.AboveNormal,
            };

            return Task.FromResult(Enumerable.Repeat(myMock, 3));
        }
    }

    public sealed class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSitemap(options =>
            {
                options.BaseUrl = "http://localhost/";
                options.SitemapPath = "/sitemap";
                options.SetDynamicProvider<MyCustomSitemapProvider>()
                    .ForUserAgent("*")
                    .Allow("/");
            });

            services.AddControllersWithViews();
        }

       
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseSitemap();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
