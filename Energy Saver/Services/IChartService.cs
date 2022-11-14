using ChartJSCore.Models;
using Energy_Saver.Model;
using static Energy_Saver.Services.ChartService;

namespace Energy_Saver.Services
{
    public interface IChartService
    {
        public Chart CreateChart<T>(List<List<Taxes>> tableData, Enums.ChartType chartType, List<T> values, List<FilterTypes> filters, int year);
    }
}
