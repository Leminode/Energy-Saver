using Energy_Saver.Model;
using Energy_Saver.Services;
using Energy_Saver.DataSpace;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using static Energy_Saver.Model.Utilities;
using System.Security.Claims;
using static Energy_Saver.Services.ISuggestionsService;

namespace Energy_Saver.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ISuggestionsService _suggestionsService;
        private readonly EnergySaverTaxesContext _context;

        public List<List<Taxes>>? Taxes { get; set; }

        [BindProperty]
        public List<decimal> TaxComparison { get; set; }
        public List<TaxesWithSum> IncDecTaxes { get; set; }

        public IndexModel(ILogger<IndexModel> logger, ISuggestionsService suggestionsService, EnergySaverTaxesContext context)
        {
            _logger = logger;
            _suggestionsService = suggestionsService;
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                var tempString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value.Split('|').Last();
                int userID = int.Parse(tempString);

                try
                {
                    var temp = await _context.Taxes.Where(taxes => taxes.UserID == userID).ToListAsync();

                    Taxes = OrderList(SortDirection.Descending, temp, tax => tax.Month).GroupBy(t => t.Year).Select(year => year.ToList()).ToList();
                    Taxes = OrderList(SortDirection.Descending, Taxes, taxes => taxes[0]);

                    TaxComparison = _suggestionsService.GetLatestTaxComparison(Taxes);
                    IncDecTaxes = _suggestionsService.PercetangeAboveOrBelowAverage(Taxes);
                }
                catch (Exception)
                {
                    return RedirectToPage("./Index");
                }
            }

            return Page();
        }

        public void OnPost()
        {

        }

        public TaxesWithSum GetComparison(Months month, int year)
        {
            return IncDecTaxes.Select(tax => tax).Where(withSum => withSum.Year == year && withSum.Month == month).First();
        }
    }
}