using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Energy_Saver.Data;
using Energy_Saver.Model;

namespace Energy_Saver.Pages
{
    public class InputModel : PageModel
    {
        private readonly Energy_Saver.Data.Energy_SaverContext _context;

        public InputModel(Energy_Saver.Data.Energy_SaverContext context)
        {
            _context = context;
        }

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

            _context.Taxes.Add(Taxes);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
