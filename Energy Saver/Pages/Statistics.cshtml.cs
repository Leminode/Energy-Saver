using ChartJSCore.Helpers;
using ChartJSCore.Models;
using Energy_Saver.Model;
using Energy_Saver.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Energy_Saver.Pages
{
    public class StatisticsModel : PageModel
    {
        private readonly IChartService _chartService;
        public Chart? Chart { get; set; }

        public StatisticsModel(IChartService chartService)
        {
            _chartService = chartService;
        }

        public void OnGet()
        {
            Chart = _chartService.CreateChart();
        }
    }
}
