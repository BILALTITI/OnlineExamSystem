using Microsoft.AspNetCore.Mvc;

namespace OnlineExamSystem.Areas.Admins.Controllers
{
    [Area("Admins")]
    [Route("Admins/[controller]/[action]")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
