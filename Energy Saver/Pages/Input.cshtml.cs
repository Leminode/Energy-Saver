using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Energy_Saver.Model;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace Energy_Saver.Pages
{
    public class InputModel : PageModel
    {
        DefaultContractResolver contractResolverCamel = new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        };

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Taxes Taxes { get; set; }
       
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            WriteToFile();

            return RedirectToPage("./Index");
        }

        public void WriteToFile()
        {
            string path = "Resources/Taxes.json";
            string json = System.IO.File.ReadAllText(Path.GetFullPath(path));

            List<Taxes>? temp = JsonConvert.DeserializeObject<List<Taxes>>(json, new JsonSerializerSettings
            {
                ContractResolver = contractResolverCamel
            });

            temp.Add(Taxes);

            string serializedString = JsonConvert.SerializeObject(temp, Formatting.Indented, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            System.IO.File.WriteAllText(Path.GetFullPath(path), serializedString);
        }
    }
}
