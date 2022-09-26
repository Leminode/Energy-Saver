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
        public List<List<Taxes>> ?Taxes { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        

        public void OnGet()
        {
            string path = "Resources/Taxes.json";
            string json = System.IO.File.ReadAllText(Path.GetFullPath(path));

            List<Taxes> temp = JsonConvert.DeserializeObject<List<Taxes>>(json, new JsonSerializerSettings
            {
                ContractResolver = contractResolver
            });
            Taxes = temp.GroupBy(t => t.Year).Select(year => year.ToList()).ToList();
            Taxes = Taxes.Select(taxes => taxes.OrderBy(i => i.Year).ToList()).OrderBy(taxes => taxes[0]).ToList();// OrderByDescending(t => t.Year).ToList();
        }

        public void OnPost()
        {

        }
    }
}