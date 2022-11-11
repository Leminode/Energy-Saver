using ChartJSCore.Helpers;
using ChartJSCore.Models;
using Energy_Saver.Model;

namespace Energy_Saver.Services
{
    public class ChartService : IChartService
    {
        private float Random { get; set; } = (float) new Random().NextDouble();

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
                        function = _ => 1;
                        break;
                }

                List<double?> monthData = new List<double?>();

                foreach (Months month in Enum.GetValues(typeof(Months)))
                {
                    double? foundData;
                    try
                    {
                        foundData = tableData.SelectMany(x => x.Where(tax => tax.Year == year && tax.Month == month).Select(function)).First();
                    } catch (InvalidOperationException) {
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
                Min = 0,
                Ticks = new Tick() { Color = ChartColor.FromRgb(169, 169, 169) },
            });

            chart.Options.Scales = dict;

            chart.Options.Plugins = new Plugins { Legend = new Legend { Labels = new LegendLabel { Color = ChartColor.FromRgb(169, 169, 169) } } };

            return chart;
        }

        private LineDataset CreateNewLineDataset(List<double?> data, string label)
        {
            ChartColor chartColor = GetRandomChartColor();
            ChartColor withAlpha = new ChartColor { Red = chartColor.Red, Blue = chartColor.Blue, Green = chartColor.Green, Alpha = 0.4 };

            LineDataset lineDataset = new LineDataset()
            {
                Label = label,
                Data = data,
                Fill = "false",
                Tension = 0.1,
                BackgroundColor = new List<ChartColor> { withAlpha },
                BorderColor = new List<ChartColor> { chartColor },
                BorderCapStyle = "butt",
                BorderDash = new List<int>(),
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

        private ChartColor GetRandomChartColor(float s = 0.5F, float v = 0.95F)
        {
            float golden_ratio = 0.618033988749895F;

            Func<float, byte> f = delegate (float n)
            {
                float k = (n + Random * 6) % 6;
                return (byte)((v - (v * s * (Math.Max(0, Math.Min(Math.Min(k, 4 - k), 1))))) * 255);
            };

            Random += golden_ratio;
            Random %= 1;

            return ChartColor.FromRgb(f(5), f(3), f(1));
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
