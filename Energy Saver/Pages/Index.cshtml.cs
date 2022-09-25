using Energy_Saver.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Energy_Saver.Pages
{
    public class IndexModel : PageModel
    {
        DefaultContractResolver contractResolver = new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        };
        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        public List<Taxes> ?Taxes { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        

        public void OnGet()
        {
            string path = "Resources/Taxes.json";
            string json = System.IO.File.ReadAllText(Path.GetFullPath(path));

            Taxes = JsonConvert.DeserializeObject<List<Taxes>>(json, new JsonSerializerSettings
            {
                ContractResolver = contractResolver
            });
            Taxes = Taxes.OrderByDescending(t => t.Year).ToList();
        }

        public void OnPost()
        {

        }
    }
}