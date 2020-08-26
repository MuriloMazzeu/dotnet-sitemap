using System.IO;
using System.Text;

namespace SitemapCore.Shared
{
    internal sealed class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => new UTF8Encoding(false);
    }
}
