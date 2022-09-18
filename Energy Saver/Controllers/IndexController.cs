using Energy_Saver.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;

namespace Energy_Saver.Controllers
{
    public class IndexController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            string path = "Resource/Taxes.json";
            string readText = System.IO.File.ReadAllText(Path.GetFullPath(path));
            JArray convert = JArray.Parse(readText);
            ViewData["responsedata"] = convert;

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
