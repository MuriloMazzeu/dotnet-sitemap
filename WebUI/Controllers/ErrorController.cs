using Microsoft.AspNetCore.Mvc;
using SitemapCore;

namespace WebUI.Controllers
{
    [Route("err")]
    public class ErrorController : Controller
    {
        [HttpGet("internal")]
        [Sitemap(Priority = SitemapPriority.Lowest, LastModification = "2020-08-25")]
        public IActionResult Index()
        {
            return View("Error");
        }

        [HttpGet("/test")]
        [Sitemap(Priority = SitemapPriority.Lowest, LastModification = "2020-08-25")]
        public IActionResult Test()
        {
            return View("Error");
        }
    }
}
