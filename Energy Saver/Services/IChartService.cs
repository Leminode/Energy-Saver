using ChartJSCore.Models;
using static Energy_Saver.Services.ChartService;

namespace Energy_Saver.Services
{
    public interface IChartService
    {
        public Chart CreateChart(Enums.ChartType chartType, List<DataWithLabel> tableData, List<string> labels, bool withLegend = true);
    }
}
