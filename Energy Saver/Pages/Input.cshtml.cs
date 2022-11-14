using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Energy_Saver.Model;
using Energy_Saver.Services;
using Energy_Saver.DataSpace;

namespace Energy_Saver.Pages
{
    public class InputModel : PageModel
    {
        private readonly EnergySaverTaxesContext _context;

        [BindProperty]
        public Taxes Taxes { get; set; }

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

            //SUPER TEMP SOLUTION
            Taxes.UserID = 4;

            _context.Taxes.Add(Taxes);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        /*private readonly ITableService _tableService;

        public InputModel(ITableService tableService)
        {
            _tableService = tableService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Taxes Taxes { get; set; }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _tableService.AddEntry(Taxes);

            return RedirectToPage("./Index");
        }*/
    }
}
