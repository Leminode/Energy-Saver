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
        private readonly ISuggestionsService _suggestionsService;


        [BindProperty]
        public List<List<Taxes>>? Taxes { get; set; }

        [BindProperty]
        public decimal gasComparison { get; set; }

        [BindProperty]
        public decimal waterComparison { get; set; }

        [BindProperty]
        public decimal electricityComparison { get; set; }

        [BindProperty]
        public decimal heatingComparison { get; set; }

        public IndexModel(ILogger<IndexModel> logger, ITableService tableService, ISuggestionsService suggestionsService)
        {
            _logger = logger;
            _tableService = tableService;
            _suggestionsService = suggestionsService;
        }

        public void OnGet()
        {
            Taxes = _tableService.GetTableContents();

            gasComparison = _suggestionsService.GetLatestGasComparison();
            waterComparison = _suggestionsService.GetLatestWaterComparison();
            electricityComparison = _suggestionsService.GetLatestElectricityComparison();
            heatingComparison = _suggestionsService.GetLatestHeatingComparison();
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