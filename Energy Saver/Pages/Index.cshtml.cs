using Energy_Saver.Model;
using Energy_Saver.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Energy_Saver.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ITableService _tableService;

        [BindProperty]
        public List<List<Taxes>>? Taxes { get; set; }

        public IndexModel(ILogger<IndexModel> logger, ITableService tableService)
        {
            _logger = logger;
            _tableService = tableService;
        }

        public void OnGet()
        {
            Taxes = _tableService.GetTableContents();
        }

        public void OnPost()
        {

        }

        public IActionResult OnPostDelete(int index, int yearIndex)
        {
            _tableService.DeleteEntry(yearIndex: yearIndex, monthIndex: index);

            return RedirectToPage();
        }
    }
}