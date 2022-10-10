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

            this.WriteToFile(Taxes);

            return RedirectToPage("./Index");
        }
    }
}
