using Microsoft.AspNetCore.Mvc;

namespace Energy_Saver.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
