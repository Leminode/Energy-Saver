using ChartJSCore.Helpers;
using ChartJSCore.Models;
using System.Diagnostics.CodeAnalysis;

namespace Energy_Saver.Services
{
    public class ChartService : IChartService
    {
        private float Random { get; set; } = (float) new Random().NextDouble();

        [ExcludeFromCodeCoverage]
        public Chart CreateChart(Enums.ChartType chartType, List<DataWithLabel> tableData, List<string> labels, bool withLegend = true)
        {
            Chart chart = new Chart();
            chart.Type = chartType;

            Data data = new Data();
            data.Datasets = new List<Dataset>();

            data.Labels = labels;

            foreach (DataWithLabel dataWithLabel in tableData)
            {
                switch (chartType)
                {
                    case Enums.ChartType.Line:
                        data.Datasets.Add(CreateNewLineDataset(dataWithLabel.Data, dataWithLabel.Label));
                        break;
                    case Enums.ChartType.Bar:
                        data.Datasets.Add(CreateNewBarDataset(dataWithLabel.Data, dataWithLabel.Label));
                        break;
                }
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
            if (withLegend)
            {
                chart.Options.Plugins = new Plugins { Legend = new Legend { Labels = new LegendLabel { Color = ChartColor.FromRgb(169, 169, 169) } } };
            } else
            {
                chart.Options.Plugins = new Plugins { Legend = new Legend { Display = false } };
            }
            

            return chart;
        }

        [ExcludeFromCodeCoverage]
        public LineDataset CreateNewLineDataset(List<double?> data, string label)
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

        [ExcludeFromCodeCoverage]
        public BarDataset CreateNewBarDataset(List<double?> data, string label)
        {
            var backGroundColor = new List<ChartColor>();
            var borderColor = new List<ChartColor>();

            foreach (double? value in data)
            {
                var chartColor = GetRandomChartColor();
                var withAlpha = new ChartColor { Red = chartColor.Red, Blue = chartColor.Blue, Green = chartColor.Green, Alpha = 0.4 };

                backGroundColor.Add(withAlpha);
                borderColor.Add(chartColor);
            }

            var barDataset = new BarDataset
            {
                Label = label,
                Data = data,
                BackgroundColor = backGroundColor,
                BorderColor = borderColor,
                BorderWidth = new List<int>() { 1 },
                BarPercentage = 0.5
            };

            return barDataset;
        }

        public ChartColor GetRandomChartColor(float s = 0.5F, float v = 0.95F)
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

        public struct DataWithLabel
        {
            public string Label { get; set; }
            public List<double?> Data { get; set; }
        }
    }
}
