using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Energy_Saver.Model;
using Energy_Saver.Services;

namespace Energy_Saver.Pages
{
    public class InputModel : PageModel
    {
        private readonly ITableService _tableService;

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
        }
    }
}
