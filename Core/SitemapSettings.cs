using System;
using System.Collections.Generic;

namespace SitemapCore
{
    public sealed class SitemapSettings
    {
        internal SitemapSettings()
        {
            BaseUrl = "/";
            DefaultAction = "Index";
            DefaultController = "Home";
            SitemapPath = "/sitemap.xml";
            UserAgents = new Dictionary<string, UserAgentSettings>();
        }

        public string SitemapPath { get; set; }
        public string BaseUrl { get; set; }
        public string DefaultController { get; set; }
        public string DefaultAction { get; set; }

        internal Type SitemapProvider { get; set; }

        /// <summary>
        /// Your custom way to inform dynamic urls
        /// </summary>
        /// <typeparam name="T">Provider class with D.I. support</typeparam>
        /// <returns>Instance for chaining</returns>
        public SitemapSettings SetDynamicProvider<T>() where T : ISitemapProvider
        {
            SitemapProvider = typeof(T);
            return this;
        }

        private Dictionary<string, UserAgentSettings> UserAgents { get; }

        internal IEnumerable<UserAgentSettings> GetUserAgents()
        {
            foreach (var item in UserAgents.Values)
            {
                yield return item;
            }
        }

        /// <summary>
        /// Start robots.txt config
        /// </summary>
        /// <param name="userAgent">Use * for all agents</param>
        /// <returns>Instance for chaining</returns>
        public UserAgentSettings ForUserAgent(string userAgent)
        {
            if (!UserAgents.ContainsKey(userAgent))
            {
                var item = new UserAgentSettings(userAgent);
                UserAgents.Add(userAgent, item);
                return item;
            }
            else return UserAgents[userAgent];
        }
    }

    public sealed class UserAgentSettings
    {
        internal UserAgentSettings(string userAgent)
        {
            UserAgent = userAgent;
            _allows = new List<string>();
            _disallows = new List<string>();
        }

        private List<string> _allows { get; }
        private List<string> _disallows { get; }

        internal string UserAgent { get; }
        internal IEnumerable<string> Allows => _allows.AsReadOnly();
        internal IEnumerable<string> Disallows => _disallows.AsReadOnly();

        public UserAgentSettings Allow(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            _allows.Add(path);
            return this;
        }

        public UserAgentSettings Disallow(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            _disallows.Add(path);
            return this;
        }
    }
}
