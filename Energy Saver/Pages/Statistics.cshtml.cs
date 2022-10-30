using ChartJSCore.Helpers;
using ChartJSCore.Models;
using Energy_Saver.Model;
using Energy_Saver.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Energy_Saver.Pages
{
    public class StatisticsModel : PageModel
    {
        private readonly IChartService _chartService;
        public Chart? Chart { get; set; }
        private List<ChartService.FilterTypes> allFilters = new List<ChartService.FilterTypes>
        {
            ChartService.FilterTypes.Gas,
            ChartService.FilterTypes.Electricity,
            ChartService.FilterTypes.Water,
            ChartService.FilterTypes.Heating
        };
        [BindProperty]
        public int selectedYear { get; set; } = 2022;

        public StatisticsModel(IChartService chartService)
        {
            _chartService = chartService;
        }

        public void OnGet()
        {
            Chart = _chartService.CreateChart<Taxes>(Enums.ChartType.Line, new List<Taxes>(), allFilters, selectedYear);
        }

        public IActionResult OnPostYear(int selectedYear)
        {
            Chart = _chartService.CreateChart<Taxes>(Enums.ChartType.Line, new List<Taxes>(), allFilters, selectedYear);

            return new JsonResult(Chart.SerializeBody());
        }
    }

    
}
