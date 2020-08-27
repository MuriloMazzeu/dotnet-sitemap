using Microsoft.AspNetCore.Http;
using SitemapCore.Shared;
using System.Text;
using System.Threading.Tasks;

namespace SitemapCore.Middlewares
{
    public sealed class RobotsMiddleware : IMiddleware
    {
        private const string TYPE = "text/plain";
        private const string PATH = "/robots.txt";

        public RobotsMiddleware(SitemapSettings settings, ILocationHelper locationHelper)
        {
            var sw = new Utf8StringWriter();
            foreach (var item in settings.GetUserAgents())
            {
                sw.Write("User-agent: {0}\n", item.UserAgent);
                
                foreach (var allow in item.Allows)
                {
                    sw.Write("Allow: {0}\n", allow);
                }

                foreach (var disallow in item.Disallows)
                {
                    sw.Write("Disallow: {0}\n", disallow);
                }

                sw.WriteLine();
            }

            sw.Write("Sitemap: " + locationHelper.GetSitemapUrl(settings.SitemapPath));
            Content = sw.ToString();
        }

        private string Content { get; }

        private void SetCommonHeaders(IHeaderDictionary headers)
        {
            headers.Append("Cache-Control", "public, max-age=31536000");
        }

        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if(context.Request.Path.HasValue)
            {
                var response = context.Response;
                var path = context.Request.Path.Value.ToLowerInvariant();
                if (path == PATH)
                {
                    response.StatusCode = 200;
                    response.ContentType = TYPE;
                    SetCommonHeaders(response.Headers);
                    return response.WriteAsync(Content, Encoding.UTF8);
                }
            }

            return next(context);
        }
    }
}
