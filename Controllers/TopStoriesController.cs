using Microsoft.AspNetCore.Mvc;

namespace HackerNews.Controllers
{
    public class TopStoriesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
