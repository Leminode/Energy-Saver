using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Energy_Saver.DataSpace;
using Energy_Saver.Model;

namespace Energy_Saver.Pages.TaxesPages
{
    public class IndexModel : PageModel
    {
        private readonly Energy_Saver.DataSpace.EnergySaverTaxesContext _context;

        public IndexModel(Energy_Saver.DataSpace.EnergySaverTaxesContext context)
        {
            _context = context;
        }

        public IList<Taxes> Taxes { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Taxes != null)
            {
                Taxes = await _context.Taxes.ToListAsync();
            }
        }
    }
}
