using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Energy_Saver.DataSpace;
using Energy_Saver.Model;
using System.Security.Claims;

namespace Energy_Saver.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly EnergySaverTaxesContext _context;

        public DeleteModel(EnergySaverTaxesContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Taxes Taxes { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Taxes == null)
            {
                return NotFound();
            }

            var tempString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value.Split('|').Last();
            int userID = int.Parse(tempString);

            var taxes = await _context.Taxes.FirstOrDefaultAsync(m => m.ID == id && m.UserID == userID);

            if (taxes == null)
            {
                return NotFound();
            }
            else 
            {
                Taxes = taxes;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Taxes == null)
            {
                return NotFound();
            }

            var taxes = await _context.Taxes.FindAsync(id);

            if (taxes != null)
            {
                Taxes = taxes;
                _context.Taxes.Remove(Taxes);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
