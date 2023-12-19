using Microsoft.AspNetCore.Mvc;

namespace NewPustok.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
