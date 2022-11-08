using Energy_Saver.Model;
using Energy_Saver.Services;
using Energy_Saver.DataSpace;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Energy_Saver.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ITableService _tableService;
        private readonly ISuggestionsService _suggestionsService;
        private readonly EnergySaverTaxesContext _context;

        //[BindProperty]
        //public List<List<Taxes>>? Taxes { get; set; }
        public IList<Taxes> Taxes { get; set; } = default!;

        [BindProperty]
        public decimal gasComparison { get; set; }

        [BindProperty]
        public decimal waterComparison { get; set; }

        [BindProperty]
        public decimal electricityComparison { get; set; }

        [BindProperty]
        public decimal heatingComparison { get; set; }

        public IndexModel(ILogger<IndexModel> logger, ITableService tableService, ISuggestionsService suggestionsService, EnergySaverTaxesContext context)
        {
            _logger = logger;
            _tableService = tableService;
            _suggestionsService = suggestionsService;
            _context = context;
        }

        public async Task OnGetAsync()
        {
            if (_context.Taxes != null)
            {
                Taxes = await _context.Taxes.ToListAsync();
            }
        }

        /*public void OnGet()
        {
            Taxes = _tableService.GetTableContents();

            gasComparison = _suggestionsService.GetLatestGasComparison();
            waterComparison = _suggestionsService.GetLatestWaterComparison();
            electricityComparison = _suggestionsService.GetLatestElectricityComparison();
            heatingComparison = _suggestionsService.GetLatestHeatingComparison();
        }*/

        public void OnPost()
        {

        }

        /*public IActionResult OnPostDelete(int index, int yearIndex)
        {
            _tableService.DeleteEntry(yearIndex: yearIndex, monthIndex: index);

            return RedirectToPage();
        }*/
    }
}