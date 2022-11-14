using ChartJSCore.Helpers;
using ChartJSCore.Models;
using Energy_Saver.DataSpace;
using Energy_Saver.Model;
using Energy_Saver.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using static Energy_Saver.Model.Serialization;

namespace Energy_Saver.Pages
{
    public class StatisticsModel : PageModel
    {
        private readonly IChartService _chartService;
        private readonly EnergySaverTaxesContext _context;

        public Chart? Chart { get; set; }
        private List<List<Taxes>>? Taxes { get; set; }

        private List<ChartService.FilterTypes> allFilters = new List<ChartService.FilterTypes>
        {
            ChartService.FilterTypes.Gas,
            ChartService.FilterTypes.Electricity,
            ChartService.FilterTypes.Water,
            ChartService.FilterTypes.Heating
        };

        public StatisticsModel(IChartService chartService, EnergySaverTaxesContext context)
        {
            _chartService = chartService;
            _context = context;
        }

        public void OnGet()
        {
            List<Taxes> temp = _context.Taxes.ToList();

            Taxes = OrderList(SortDirection.Descending, temp, tax => tax.Month).GroupBy(t => t.Year).Select(year => year.ToList()).ToList();
            Taxes = OrderList(SortDirection.Descending, Taxes, taxes => taxes[0]);

            Chart = _chartService.CreateChart<Taxes>(Taxes, Enums.ChartType.Line, new List<Taxes>(), allFilters, 2022);
        }

        public async Task<IActionResult> OnPostYear(int selectedYear)
        {
            var temp = await _context.Taxes.ToListAsync();

            Taxes = OrderList(SortDirection.Descending, temp, tax => tax.Month).GroupBy(t => t.Year).Select(year => year.ToList()).ToList();
            Taxes = OrderList(SortDirection.Descending, Taxes, taxes => taxes[0]);

            Chart = _chartService.CreateChart<Taxes>(Taxes, Enums.ChartType.Line, new List<Taxes>(), allFilters, selectedYear);

            return new JsonResult(Chart.SerializeBody());
        }
    }

    
}
