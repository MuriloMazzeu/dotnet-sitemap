using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SitemapCore.Middlewares;
using SitemapCore.Shared;
using System;

namespace SitemapCore
{
    public static class StartupExtentions
    {
        public static IServiceCollection AddSitemap(this IServiceCollection services, Action<SitemapSettings> configure = null)
        {
            var settings = new SitemapSettings();
            if (configure is null)
            {
                settings.ForUserAgent("*").Allow("/");
            }
            else configure(settings);
            services.AddSingleton(settings);
            services.AddSingleton<RobotsMiddleware>();
            services.AddSingleton<SitemapMiddleware>();
            services.AddSingleton<ILocationHelper, LocationHelper>();
            services.AddSingleton(typeof(ISitemapProvider), settings.SitemapProvider);
            return services;
        }

        public static IApplicationBuilder UseSitemap(this IApplicationBuilder app)
        {
            app.UseMiddleware<SitemapMiddleware>();
            app.UseMiddleware<RobotsMiddleware>();
            return app;
        }
    }
}
