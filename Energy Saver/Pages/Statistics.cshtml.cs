using ChartJSCore.Models;
using Energy_Saver.DataSpace;
using Energy_Saver.Model;
using Energy_Saver.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using static Energy_Saver.Model.Serialization;
using static Energy_Saver.Services.ChartService;

namespace Energy_Saver.Pages
{
    public class StatisticsModel : PageModel
    {
        public delegate void StatisticsTaxesHandler(object source, NotificationService.NotificationArgs args);
        public event StatisticsTaxesHandler TaxGet;

        private readonly IChartService _chartService;
        private readonly EnergySaverTaxesContext _context;
        private readonly INotificationService _notificationService;

        public Chart? YearChart { get; set; }
        public Chart? MonthChart { get; set; }
        [BindProperty]
        public List<List<Taxes>>? Taxes { get; set; }
        private Lazy<List<string>> MonthLabels { get; set; } = new Lazy<List<string>>(() => GenerateMonthsLabels());
        private Lazy<List<string>> TaxLabels { get; set; } = new Lazy<List<string>>(() => GenerateTaxLabels());


        public StatisticsModel(IChartService chartService, EnergySaverTaxesContext context, INotificationService notificationService)
        {
            _chartService = chartService;
            _context = context;
            _notificationService = notificationService;
            TaxGet += _notificationService.CreateNotification;
        }

        public void OnGet()
        {
            Taxes = GetTaxesFromDatabase();

            YearChart = _chartService.CreateChart(Enums.ChartType.Line, CreateDataForYearChart(2022), MonthLabels.Value);
            MonthChart = _chartService.CreateChart(Enums.ChartType.Bar, CreateDataForMonthChart(Months.January, 2022), TaxLabels.Value, false);
        }

        public IActionResult OnPostYear(int selectedYear, string selectedType)
        {
            Taxes = GetTaxesFromDatabase();

            var monthType = Enum.Parse<Enums.ChartType>(selectedType);

            YearChart = _chartService.CreateChart(Enums.ChartType.Line, CreateDataForYearChart(selectedYear), MonthLabels.Value);
            MonthChart = _chartService.CreateChart(monthType, CreateDataForMonthChart(Months.January, selectedYear), TaxLabels.Value, false);

            return new JsonResult(new { yearChart = YearChart.SerializeBody(), monthChart = MonthChart.SerializeBody() });
        }

        public IActionResult OnPostMonth(string selectedMonth, int selectedYear, string selectedType)
        {
            Taxes = GetTaxesFromDatabase();

            var month = Enum.Parse<Months>(selectedMonth);
            var type = Enum.Parse<Enums.ChartType>(selectedType);

            MonthChart = _chartService.CreateChart(type, CreateDataForMonthChart(month, selectedYear), TaxLabels.Value, false);

            return new JsonResult(MonthChart.SerializeBody());
        }

        private List<DataWithLabel> CreateDataForYearChart(int year)
        {
            var flattenedList = new List<DataWithLabel>();

            foreach (FilterTypes filter in Enum.GetValues(typeof(FilterTypes)))
            {
                var data = new DataWithLabel { Label = filter.ToString(), Data = new List<double?>() };

                Func<Taxes, double?> function;
                switch (filter)
                {
                    case FilterTypes.Gas:
                        function = tax => (double)tax.GasAmount;
                        break;
                    case FilterTypes.Electricity:
                        function = tax => (double)tax.ElectricityAmount;
                        break;
                    case FilterTypes.Water:
                        function = tax => (double)tax.WaterAmount;
                        break;
                    case FilterTypes.Heating:
                        function = tax => (double)tax.HeatingAmount;
                        break;
                    default:
                        function = _ => 1;
                        break;
                }

                foreach (Months month in Enum.GetValues(typeof(Months)))
                {
                    double? foundData = Taxes
                        .SelectMany(x => x.Where(tax => tax.Year == year && tax.Month == month)
                        .Select(function))
                        .FirstOrDefault();
                    data.Data.Add(foundData);
                }

                flattenedList.Add(data);
            }

            return flattenedList;
        }

        private List<DataWithLabel> CreateDataForMonthChart(Months month, int year)
        {
            var monthData = new List<DataWithLabel>();

            var data = new DataWithLabel { Label = month.ToString(), Data = new List<double?>() };

            foreach (FilterTypes filter in Enum.GetValues(typeof(FilterTypes)))
            {
                Func<Taxes, double?> function;
                switch (filter)
                {
                    case FilterTypes.Gas:
                        function = tax => (double)tax.GasAmount;
                        break;
                    case FilterTypes.Electricity:
                        function = tax => (double)tax.ElectricityAmount;
                        break;
                    case FilterTypes.Water:
                        function = tax => (double)tax.WaterAmount;
                        break;
                    case FilterTypes.Heating:
                        function = tax => (double)tax.HeatingAmount;
                        break;
                    default:
                        function = _ => 1;
                        break;
                }

                double? foundData = Taxes
                    .SelectMany(x => x.Where(tax => tax.Year == year && tax.Month == month)
                    .Select(function))
                    .FirstOrDefault();
                data.Data.Add(foundData);
            }
            monthData.Add(data);


            return monthData;
        }

        private static List<string> GenerateTaxLabels()
        {
            var filters = new List<string>();
            foreach (FilterTypes filter in Enum.GetValues(typeof(FilterTypes)))
            {
                filters.Add(filter.ToString());
            }

            return filters;
        }

        private static List<string> GenerateMonthsLabels()
        {
            var months = new List<string>();
            foreach (Months month in Enum.GetValues(typeof(Months)))
            {
                months.Add(month.ToString());
            }

            return months;
        }

        private List<List<Taxes>>? GetTaxesFromDatabase()
        {
            if (User.Identity.IsAuthenticated)
            {
                var tempString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value.Split('|').Last();
                int userID = int.Parse(tempString);

                try
                {
                    var temp = _context.Taxes.Where(taxes => taxes.UserID == userID).ToList();

                var taxes = OrderList<Taxes, Months>(SortDirection.Descending, temp, tax => tax.Month)
                    .GroupBy(t => t.Year)
                    .Select(year => year.ToList())
                    .ToList();
                taxes = OrderList(SortDirection.Descending, taxes, taxes => taxes[0]);

                    return taxes;
                }
                catch (Exception)
                {
                    OnTaxGetError();
                    RedirectToPage("./Index");
                }
            }

            return null;

        }

        protected virtual void OnTaxGetError()
        {
            TaxGet?.Invoke(this, new NotificationService.NotificationArgs
            {
                Message = "Could not retrieve tax list",
                Type = NotificationService.NotificationType.Error
            });
        }
    }


}
