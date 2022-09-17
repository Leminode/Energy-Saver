using Energy_Saver.Model;
using Microsoft.AspNetCore.Mvc;

namespace Energy_Saver.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult TaxDetails()
        {
            List<Taxes> ObjTax = new List<Taxes>()
            {

            };

            return Json(ObjTax);
        }
    }
}
