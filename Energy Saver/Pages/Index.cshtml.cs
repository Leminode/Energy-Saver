using Energy_Saver.Model;
using Energy_Saver.Services;
using Energy_Saver.DataSpace;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using static Energy_Saver.Model.Serialization;

namespace Energy_Saver.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ISuggestionsService _suggestionsService;
        private readonly EnergySaverTaxesContext _context;

        public List<List<Taxes>>? Taxes { get; set; }

        [BindProperty]
        public List<decimal> taxComparison { get; set; }

        public IndexModel(ILogger<IndexModel> logger, ISuggestionsService suggestionsService, EnergySaverTaxesContext context)
        {
            _logger = logger;
            _suggestionsService = suggestionsService;
            _context = context;
        }

        public async Task OnGetAsync()
        {
            var temp = await _context.Taxes.ToListAsync();

            Taxes = OrderList(SortDirection.Descending, temp, tax => tax.Month).GroupBy(t => t.Year).Select(year => year.ToList()).ToList();
            Taxes = OrderList(SortDirection.Descending, Taxes, taxes => taxes[0]);

            taxComparison = _suggestionsService.GetLatestTaxComparison(Taxes);
        }

        public void OnPost()
        {

        }
    }
}