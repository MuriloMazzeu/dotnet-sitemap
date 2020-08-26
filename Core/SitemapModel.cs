using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SitemapCore
{
    [XmlRoot("urlset", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
    public sealed class SitemapRoot
    {
        [XmlElement("url")]
        public SitemapItem[] Urls { get; set; }
    }

    public sealed class SitemapItem
    {
        public SitemapItem()
        {

        }

        public SitemapItem(Sitemap item)
        {
            Location = item.Location;
            Priority = item.Priority.ToDescription();
            ChangeFrequency = item.ChangeFrequency.ToDescription();
            LastModification = item.LastModification.HasValue
                ? item.LastModification.Value.ToString("yyyy-MM-dd")
                : null;
        }

        [XmlElement("loc")]
        public string Location { get; set; }

        [XmlElement("lastmod")]
        public string LastModification { get; set; }

        [XmlElement("priority")]
        public string Priority { get; set; }
        
        [XmlElement("changefreq")]
        public string ChangeFrequency { get; set; }
    }

    public sealed class Sitemap
    {
        /// <param name="location">must be less than 2,048 characters and begin with the protocol (such as http)</param>
        public Sitemap(string location)
        {
            Location = location;
            LastModification = null;
            Priority = SitemapPriority.Normal;
        }

        public string Location { get; }

        public DateTime? LastModification { get; set; }

        public SitemapPriority Priority { get; set; }

        public ChangeFrequency ChangeFrequency { get; set; }
    }

    public enum SitemapPriority
    {
        [Description("")]
        None = 0,

        [Description("0.1")]
        Lowest = 1,

        [Description("0.3")]
        BelowNormal = 2,

        [Description("0.5")]
        Normal = 3,

        [Description("0.7")]
        AboveNormal = 4,

        [Description("0.9")]
        Highest = 5,
    }

    public enum ChangeFrequency
    {

        [Description("")]
        None = 0,

        [Description("always")]
        Always = 1,

        [Description("hourly")]
        Hourly = 2,

        [Description("daily")]
        Daily = 3,

        [Description("weekly")]
        Weekly = 4,

        [Description("monthly")]
        Monthly = 5,

        [Description("anual")]
        Anual = 6,

        [Description("never")]
        Never = 7,
    }

    public static class EnumExtentions
    {
        public static string ToDescription(this Enum e) => e.GetType().GetField(e.ToString())
            .GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] a && a.Length > 0 
            ? a[0].Description == string.Empty ? null : a[0].Description
            : e.ToString();
    }
}
