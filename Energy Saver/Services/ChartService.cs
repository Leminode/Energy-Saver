using ChartJSCore.Helpers;
using ChartJSCore.Models;
using Energy_Saver.Model;

namespace Energy_Saver.Services
{
    public class ChartService : IChartService
    {
        public Chart CreateChart<T>(Enums.ChartType chartType, List<T> values, List<FilterTypes> filters, int year)
        {
            Chart chart = new Chart();
            List<List<Taxes>> tableData = Serialization.ReadFromFile();
            chart.Type = chartType;

            List<string> labels = new List<string>();

            Data data = new Data();
            data.Datasets = new List<Dataset>();

            foreach(Months month in Enum.GetValues(typeof(Months)))
            {
                labels.Add(month.ToString());
            }

            data.Labels = labels;

            foreach (FilterTypes filter in filters)
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
                        function = tax => 1;
                        break;
                }

                List<double?> monthData = new List<double?>();

                foreach (Months month in Enum.GetValues(typeof(Months)))
                {
                    double? foundData;
                    try
                    {
                        foundData = tableData.SelectMany(x => x.Where(tax => tax.Year == year && tax.Month == month).Select(function)).First();
                    } catch (InvalidOperationException e) {
                        foundData = 0;
                    }
                    monthData.Add(foundData);
                }

                data.Datasets.Add(CreateNewLineDataset(monthData, filter.ToString()));
            }

            chart.Data = data;
            
            Dictionary<string, Scale> dict = new Dictionary<string, Scale>();
            dict.Add("x", new Scale() 
            { 
                Grid = new Grid() 
                { 
                    Color = new List<ChartColor> { ChartColor.FromRgb(169, 169, 169) } 
                },
                Ticks = new Tick() { Color = ChartColor.FromRgb(169, 169, 169) }
            });
            dict.Add("y", new Scale() 
            { 
                Grid = new Grid() { Color = new List<ChartColor> { ChartColor.FromRgb(169, 169, 169) } }, 
                Ticks = new Tick() { Color = ChartColor.FromRgb(169, 169, 169)}
            });

            chart.Options.Scales = dict;

            return chart;
        }

        private LineDataset CreateNewLineDataset(List<double?> data, string label)
        {
            byte[] color = GetRandomChartColor();
            ChartColor chartColor = ChartColor.FromRgb(color[0], color[1], color[2]);

            LineDataset lineDataset = new LineDataset()
            {
                Label = label,
                Data = data,
                Fill = "false",
                Tension = 0.1,
                BackgroundColor = new List<ChartColor> { ChartColor.FromRgba(color[0], color[1], color[2], 0.4) },
                BorderColor = new List<ChartColor> { chartColor },
                BorderCapStyle = "butt",
                BorderDash = new List<int> { },
                BorderDashOffset = 0.0,
                BorderJoinStyle = "miter",
                PointBorderColor = new List<ChartColor> { chartColor },
                PointBackgroundColor = new List<ChartColor> { chartColor },
                PointBorderWidth = new List<int> { 1 },
                PointHoverRadius = new List<int> { 5 },
                PointHoverBackgroundColor = new List<ChartColor> { chartColor },
                PointHoverBorderColor = new List<ChartColor> { chartColor },
                PointHoverBorderWidth = new List<int> { 2 },
                PointRadius = new List<int> { 1 },
                PointHitRadius = new List<int> { 10 },
                SpanGaps = false
            };

            return lineDataset;
        }

        private byte[] GetRandomChartColor()
        {
            byte[] random = new byte[3];
            new Random().NextBytes(random);

            return random;
        }

        public enum FilterTypes
        {
            Gas,
            Electricity,
            Water,
            Heating
        }
    }
}
