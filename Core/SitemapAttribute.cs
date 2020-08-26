using System;

namespace SitemapCore
{
    public sealed class SitemapAttribute : Attribute
    {
        /// <summary>
        /// (Optional)
        /// </summary>
        public SitemapPriority Priority { get; set; }

        /// <summary>
        /// (Optional) In format YYYY-MM-DD
        /// </summary>
        public string LastModification { get; set; }

        /// <summary>
        /// (Optional)
        /// </summary>
        public ChangeFrequency ChangeFrequency { get; set; }
    }
}
