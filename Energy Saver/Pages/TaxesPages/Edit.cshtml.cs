using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Energy_Saver.DataSpace;
using Energy_Saver.Model;

namespace Energy_Saver.Pages.TaxesPages
{
    public class EditModelOG : PageModel
    {
        private readonly Energy_Saver.DataSpace.EnergySaverTaxesContext _context;

        public EditModelOG(Energy_Saver.DataSpace.EnergySaverTaxesContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Taxes Taxes { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Taxes == null)
            {
                return NotFound();
            }

            var taxes =  await _context.Taxes.FirstOrDefaultAsync(m => m.ID == id);
            if (taxes == null)
            {
                return NotFound();
            }
            Taxes = taxes;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Taxes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaxesExists(Taxes.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool TaxesExists(int id)
        {
          return _context.Taxes.Any(e => e.ID == id);
        }
    }
}
