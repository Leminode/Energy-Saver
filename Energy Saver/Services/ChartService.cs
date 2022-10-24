using ChartJSCore.Helpers;
using ChartJSCore.Models;
using Energy_Saver.Model;

namespace Energy_Saver.Services
{
    public class ChartService : IChartService
    {
        public Chart CreateChart()
        {
            Chart chart = new Chart();
            List<List<Taxes>> tableData = Serialization.ReadFromFile();
            chart.Type = Enums.ChartType.Line;

            Data data = new Data();
            data.Labels = new List<string>() { "Gas", "Electricity", "Water", "Heating" };

            LineDataset dataset = new LineDataset()
            {
                Label = "Amount payed all time",
                Data = new List<double?>
                {
                    (double)tableData.SelectMany(x => x.Select(item => item.GasAmount)).Sum(),
                    (double)tableData.SelectMany(x => x.Select(item => item.ElectricityAmount)).Sum(),
                    (double)tableData.SelectMany(x => x.Select(item => item.WaterAmount)).Sum(),
                    (double)tableData.SelectMany(x => x.Select(item => item.HeatingAmount)).Sum()
                },
                Fill = "false",
                Tension = 0.1,
                BackgroundColor = new List<ChartColor> { ChartColor.FromRgba(75, 192, 192, 0.4) },
                BorderColor = new List<ChartColor> { ChartColor.FromRgb(75, 192, 192) },
                BorderCapStyle = "butt",
                BorderDash = new List<int> { },
                BorderDashOffset = 0.0,
                BorderJoinStyle = "miter",
                PointBorderColor = new List<ChartColor> { ChartColor.FromRgb(75, 192, 192) },
                PointBackgroundColor = new List<ChartColor> { ChartColor.FromHexString("#ffffff") },
                PointBorderWidth = new List<int> { 1 },
                PointHoverRadius = new List<int> { 5 },
                PointHoverBackgroundColor = new List<ChartColor> { ChartColor.FromRgb(75, 192, 192) },
                PointHoverBorderColor = new List<ChartColor> { ChartColor.FromRgb(220, 220, 220) },
                PointHoverBorderWidth = new List<int> { 2 },
                PointRadius = new List<int> { 1 },
                PointHitRadius = new List<int> { 10 },
                SpanGaps = false
            };

            data.Datasets = new List<Dataset>();
            data.Datasets.Add(dataset);

            chart.Data = data;

            return chart;
        }
    }
}
