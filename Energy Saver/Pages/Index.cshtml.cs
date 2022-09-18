using Energy_Saver.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Energy_Saver.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public Records Records { get; set; }
        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        public List<Taxes> Taxes { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        

        public void OnGet()
        {
            string path = "Resources/Taxes.json";
            string json = System.IO.File.ReadAllText(Path.GetFullPath(path));
            Taxes = JsonConvert.DeserializeObject<List<Taxes>>(json);
        }

        public void OnPost()
        {

        }
    }
}