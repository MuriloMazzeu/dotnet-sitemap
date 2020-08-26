using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using SitemapCore.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SitemapCore.Middlewares
{
    public sealed class SitemapMiddleware : IMiddleware
    {
        private const string SITEMAP_TYPE = "text/xml";

        public SitemapMiddleware(SitemapSettings a, ISitemapProvider b, IActionDescriptorCollectionProvider c, ILocationHelper d)
        {
            Settings = a;
            DataProvider = b;
            ActionDescriptors = c.ActionDescriptors.Items;
            LocationHelper = d;
            Serializer = new XmlSerializer(typeof(SitemapRoot));
            Namespaces = new XmlSerializerNamespaces();
            Namespaces.Add(string.Empty, "http://www.sitemaps.org/schemas/sitemap/0.9");
        }

        private IReadOnlyList<ActionDescriptor> ActionDescriptors { get; }
        private ISitemapProvider DataProvider { get; }
        private ILocationHelper LocationHelper { get; }
        private XmlSerializer Serializer { get; }
        private SitemapSettings Settings { get; }
        private XmlSerializerNamespaces Namespaces { get; }

        private async Task<SitemapRoot> GetContentAsync()
        {
            var items = new List<SitemapItem>();
            var dynamicItems = await DataProvider.InvokeAsync();
            items.AddRange(dynamicItems.Select(i => new SitemapItem(i)));

            foreach (var item in ActionDescriptors)
            {
                var metadata = item.EndpointMetadata.FirstOrDefault(o => typeof(SitemapAttribute).IsAssignableFrom(o.GetType()));
                if (metadata is null) continue;

                var routeTemplate = item.AttributeRouteInfo?.Template;
                var routeAction = item.RouteValues.FirstOrDefault(i => i.Key == "action").Value;
                var routeController = item.RouteValues.FirstOrDefault(i => i.Key == "controller").Value;

                string location;
                if (string.IsNullOrWhiteSpace(routeTemplate))
                {
                    var controller = routeController == Settings.DefaultController ? string.Empty : routeController;
                    var action = routeAction == Settings.DefaultAction ? string.Empty : "/" + routeAction;
                    location = LocationHelper.GetSitemapUrl(controller + action);
                }
                else location = LocationHelper.GetSitemapUrl(routeTemplate);

                var attr = metadata as SitemapAttribute;
                items.Add(new SitemapItem()
                {
                    LastModification = attr.LastModification,
                    ChangeFrequency = attr.ChangeFrequency.ToDescription(),
                    Priority = attr.Priority.ToDescription(),
                    Location = location
                });
            }

            return new SitemapRoot()
            {
                Urls = items.ToArray()
            };
        }

        private string Format(SitemapRoot root)
        {
            using var sw = new Utf8StringWriter();
            using var xw = XmlWriter.Create(sw, new XmlWriterSettings()
            {
                Encoding = Encoding.UTF8,
                Indent = false,
            });

            Serializer.Serialize(xw, root, Namespaces);
            return sw.ToString();
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.Request.Path.HasValue)
            {
                var response = context.Response;
                if (context.Request.Path.Value.Equals(Settings.SitemapPath, StringComparison.OrdinalIgnoreCase))
                {
                    var content = await GetContentAsync();

                    response.StatusCode = 200;
                    response.ContentType = SITEMAP_TYPE;
                    await response.WriteAsync(Format(content));
                }
            }
            
            await next(context);
        }
    }
}
