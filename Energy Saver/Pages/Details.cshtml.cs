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
    public class DetailsModel : PageModel
    {
        private readonly EnergySaverTaxesContext _context;

        public DetailsModel(EnergySaverTaxesContext context)
        {
            _context = context;
        }

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
    }
}
