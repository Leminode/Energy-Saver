using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Energy_Saver.Model;
using Energy_Saver.Services;
using Energy_Saver.DataSpace;
using System.Security.Claims;

namespace Energy_Saver.Pages
{
    public class InputModel : PageModel
    {
        private readonly EnergySaverTaxesContext _context;

        [BindProperty]
        public Taxes? Taxes { get; set; }

        public InputModel(EnergySaverTaxesContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //will need a better implementation
            var tempString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value.Split('|').Last();
            Taxes.UserID = int.Parse(tempString);

            _context.Taxes.Add(Taxes);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
