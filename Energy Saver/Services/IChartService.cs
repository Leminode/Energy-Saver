using ChartJSCore.Models;
using static Energy_Saver.Services.ChartService;

namespace Energy_Saver.Services
{
    public interface IChartService
    {
        public Chart CreateChart<T>(Enums.ChartType chartType, List<T> values, List<FilterTypes> filters, int year);
    }
}
