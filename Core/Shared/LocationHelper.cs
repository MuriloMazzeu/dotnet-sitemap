using System;

namespace SitemapCore.Shared
{
    public interface ILocationHelper
    {
        string GetSitemapUrl(string path);
    }

    public sealed class LocationHelper : ILocationHelper
    {
        public LocationHelper(SitemapSettings settings)
        {
            Settings = settings;
        }

        private SitemapSettings Settings { get; }

        public string GetSitemapUrl(string path)
        {
            var ub = new UriBuilder(Settings.BaseUrl)
            {
                Path = path
            };
            
            var port = ub.Port;
            var scheme = ub.Scheme;
            var components = UriComponents.AbsoluteUri;
            if ((scheme == "https" && port == 433) || (scheme == "http" && port == 80))
            {
                components &= ~UriComponents.Port;
            }

            return ub.Uri.GetComponents(components, UriFormat.UriEscaped);
        }
    }
}
