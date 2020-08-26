using Microsoft.AspNetCore.Mvc;
using SitemapCore;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        [Sitemap(Priority = SitemapPriority.Highest, LastModification = "2020-08-25")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("privacidade")]
        [Sitemap(Priority = SitemapPriority.Normal, LastModification = "2020-08-26")]
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
