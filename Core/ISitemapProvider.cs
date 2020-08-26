using System.Collections.Generic;
using System.Threading.Tasks;

namespace SitemapCore
{
    public interface ISitemapProvider
    {
        Task<IEnumerable<Sitemap>> InvokeAsync();
    }
}
