using Energy_Saver.DataSpace;
using Energy_Saver.Pages;
using Energy_Saver.Services;
using Energy_Saver.Model;
using ChartJSCore.Helpers;
using Newtonsoft.Json;
using Moq;
using NToastNotify;
using ChartJSCore.Models;

namespace Energy_Saver.Tests
{
    public class ChartServiceTests
    {
        [Fact]
        public void GenerateMonthsLabelsTest()
        {
            using (var db = new EnergySaverTaxesContext(Utilities.TestDbContextOptions()))
            {
                ChartService chartService = new ChartService();

                var mock = new Mock<IToastNotification>();

                NotificationService notificationService = new NotificationService(mock.Object); 

                StatisticsModel statisticsModel = new StatisticsModel(chartService, db, notificationService);

                List<string> expectedMonths = new List<string>();

                var actualMonths = statisticsModel.GenerateMonthsLabels();

                foreach (Months month in Enum.GetValues(typeof(Months)))
                {
                    Assert.Equal(month.ToString(), actualMonths.ElementAt((int)month - 1));
                }
            }
        }

        [Fact]
        public void GenerateTaxLabelsTest()
        {
            using (var db = new EnergySaverTaxesContext(Utilities.TestDbContextOptions()))
            {
                ChartService chartService = new ChartService();

                var mock = new Mock<IToastNotification>();

                NotificationService notificationService = new NotificationService(mock.Object);

                StatisticsModel statisticsModel = new StatisticsModel(chartService, db, notificationService);

                List<string> expectedMonths = new List<string>();

                var actualMonths = statisticsModel.GenerateTaxLabels();

                foreach (ChartService.FilterTypes filterTypes in Enum.GetValues(typeof(ChartService.FilterTypes)))
                {
                    Assert.Equal(filterTypes.ToString(), actualMonths.ElementAt((int)filterTypes));
                }
            }
        }

        [Fact]
        public void CreateDataForYearChart_FourTaxesAndDataWithLabels_ReturnsEqualsTaxesAndDataWithLabels()
        {
            using (var db = new EnergySaverTaxesContext(Utilities.TestDbContextOptions()))
            {
                ChartService chartService = new ChartService();

                var mock = new Mock<IToastNotification>();

                NotificationService notificationService = new NotificationService(mock.Object);

                StatisticsModel statisticsModel = new StatisticsModel(chartService, db, notificationService);

                List<ChartService.DataWithLabel> expectedData = new List<ChartService.DataWithLabel>()
                {
                    new ChartService.DataWithLabel
                    {
                        Label = ChartService.FilterTypes.Gas.ToString(),
                        Data = new List<double?> { 4, 3, 2, 0, 0, 0, 1, 0, 0, 0, 0, 0 }
                    },

                    new ChartService.DataWithLabel
                    {
                        Label = ChartService.FilterTypes.Electricity.ToString(),
                        Data = new List<double?> { 4, 3, 2, 0, 0, 0, 1, 0, 0, 0, 0, 0 }
                    },

                    new ChartService.DataWithLabel
                    {
                        Label = ChartService.FilterTypes.Water.ToString(),
                        Data = new List<double?> { 4, 3, 2, 0, 0, 0, 1, 0, 0, 0, 0, 0 }
                    },

                    new ChartService.DataWithLabel
                    {
                        Label = ChartService.FilterTypes.Heating.ToString(),
                        Data = new List <double?> { 4, 3, 2, 0, 0, 0, 1, 0, 0, 0, 0, 0 }
                    }
                };

                statisticsModel.Taxes = new List<List<Taxes>>();

                List<Taxes> taxList = new List<Taxes>
                {
                    new Taxes
                    {
                        Year = 2020,
                        Month = Months.January,
                        GasAmount = 4,
                        ElectricityAmount = 4,
                        WaterAmount = 4,
                        HeatingAmount = 4
                    },

                    new Taxes
                    {
                        Year = 2020,
                        Month = Months.February,
                        GasAmount = 3,
                        ElectricityAmount = 3,
                        WaterAmount = 3,
                        HeatingAmount = 3
                    },

                    new Taxes
                    {
                        Year = 2020,
                        Month = Months.March,
                        GasAmount = 2,
                        ElectricityAmount = 2,
                        WaterAmount = 2,
                        HeatingAmount = 2
                    },

                    new Taxes
                    {
                        Year = 2020,
                        Month = Months.July,
                        GasAmount = 1,
                        ElectricityAmount = 1,
                        WaterAmount = 1,
                        HeatingAmount = 1
                    }
                };

                statisticsModel.Taxes.Add(taxList);

                var actualData = statisticsModel.CreateDataForYearChart(2020);

                var expectedDataString = JsonConvert.SerializeObject(expectedData);
                var actualDataString = JsonConvert.SerializeObject(actualData);

                Assert.Equal(expectedDataString, actualDataString);
            }
        }

        [Theory]
        [InlineData(Months.January, 1, 1, 1, 1)]
        [InlineData(Months.December, 321, 543, 82, 9832)]
        [InlineData(Months.October, 98.9, 98.1, 732.1, 89.2)]
        [InlineData(Months.July, 0, 0, 0, 0)]
        public void CreateDataForMonthChart_SingleTaxAndDataWithLabel_ReturnsEqualTaxAndDataWithLabel(Months month, decimal gas, decimal electricity, decimal water, decimal heating)
        {
            using (var db = new EnergySaverTaxesContext(Utilities.TestDbContextOptions()))
            {
                ChartService chartService = new ChartService();

                var mock = new Mock<IToastNotification>();

                NotificationService notificationService = new NotificationService(mock.Object);

                StatisticsModel statisticsModel = new StatisticsModel(chartService, db, notificationService);

                List<ChartService.DataWithLabel> expectedData = new List<ChartService.DataWithLabel>()
                {
                    new ChartService.DataWithLabel
                    {
                        Label = Months.January.ToString(),
                        Data = new List<double?> { (double)gas, (double)electricity, (double)water, (double)heating }
                    }
                };

                statisticsModel.Taxes = new List<List<Taxes>>();

                List<Taxes> taxList = new List<Taxes>
                {
                    new Taxes
                    {
                        Year = 2020,
                        Month = Months.January,
                        GasAmount = gas,
                        ElectricityAmount = electricity,
                        WaterAmount = water,
                        HeatingAmount = heating
                    },
                };

                statisticsModel.Taxes.Add(taxList);

                List<ChartService.DataWithLabel> actualData = statisticsModel.CreateDataForMonthChart(Months.January, 2020);

                var expectedDataString = JsonConvert.SerializeObject(expectedData);
                var actualDataString = JsonConvert.SerializeObject(actualData);

                Assert.Equal(expectedDataString, actualDataString);
            }
        }

        [Fact]
        public void GetRandomChartColorTest()
        {
            int actualColor;
            ChartColor chartColor;

            float[] luminance = new float[2];

            int[] r = new int[2];
            int[] g = new int[2];
            int[] b = new int[2];

            ChartService chartService = new ChartService();

            for(int i = 0; i < 2; i++)
            {
                chartColor = chartService.GetRandomChartColor();

                r[i] = chartColor.Red;
                g[i] = chartColor.Green;
                b[i] = chartColor.Blue;
            } 

            r[0] -= r[1];
            g[0] -= g[1];
            b[0] -= b[1];

            actualColor = Math.Abs(r[0] + g[0] + b[0]);

            Assert.True(actualColor <= 50);
        }

        [Fact]
        public void CreateNewBarDatasetTest()
        {
            ChartService chartService = new ChartService();

            List<double?> data = new List<double?> { 4, 3, 2, 0, 0, 0, 1, 0, 0, 0, 0, 0 };

            var actualBarDataset = chartService.CreateNewBarDataset(data, "Gas");

            BarDataset expectedBarDataSet = new BarDataset()
            {
                Label = "Gas",
                Data = data,
                BackgroundColor = actualBarDataset.BackgroundColor,
                BorderColor = actualBarDataset.BorderColor,
                BorderWidth = new List<int>() { 1 },
                BarPercentage = 0.5
            };

            var expectedBarDatasetString = JsonConvert.SerializeObject(expectedBarDataSet);
            var actualBarDatasetString = JsonConvert.SerializeObject(actualBarDataset);

            Assert.Equal(expectedBarDatasetString, actualBarDatasetString);
        }

        [Fact]
        public void CreateNewLineDatasetTest()
        {
            ChartService chartService = new ChartService();

            List<double?> data = new List<double?> { 4, 3, 2, 0, 0, 0, 1, 0, 0, 0, 0, 0 };

            var actualLineDataset = chartService.CreateNewLineDataset(data, "Gas");

            LineDataset expectedLineDataSet = new LineDataset()
            {
                Label = "Gas",
                Data = data,
                Fill = "false",
                Tension = 0.1,
                BackgroundColor = actualLineDataset.BackgroundColor,
                BorderColor = actualLineDataset.BorderColor,
                BorderCapStyle = "butt",
                BorderDash = new List<int>(),
                BorderDashOffset = 0.0,
                BorderJoinStyle = "miter",
                PointBorderColor = actualLineDataset.PointBorderColor,
                PointBackgroundColor = actualLineDataset.PointBackgroundColor,
                PointBorderWidth = new List<int> { 1 },
                PointHoverRadius = new List<int> { 5 },
                PointHoverBackgroundColor = actualLineDataset.PointHoverBackgroundColor,
                PointHoverBorderColor = actualLineDataset.PointHoverBorderColor,
                PointHoverBorderWidth = new List<int> { 2 },
                PointRadius = new List<int> { 1 },
                PointHitRadius = new List<int> { 10 },
                SpanGaps = false
            };

            var expectedLineDatasetString = JsonConvert.SerializeObject(expectedLineDataSet);
            var actualLineDatasetString = JsonConvert.SerializeObject(actualLineDataset);

            Assert.Equal(expectedLineDatasetString, actualLineDatasetString);
        }

        [Fact]
        public void CreateChartTest()
        {
            ChartService chartService = new ChartService();

            List<double?> data = new List<double?>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

            List<string> labels = new List<string>();

            foreach(Months month in Enum.GetValues(typeof(Months)))
            {
                labels.Add(month.ToString());
            }

            List<ChartService.DataWithLabel> tableData = new List<ChartService.DataWithLabel>()
            {
                new ChartService.DataWithLabel()
                {
                    Label = "Gas",
                    Data = data
                }
            };

            Chart actualChart = chartService.CreateChart(ChartJSCore.Models.Enums.ChartType.Line, tableData, labels);

            Chart expectedChart = new Chart();

            expectedChart.Type = ChartJSCore.Models.Enums.ChartType.Line;

            
        }
    }
}
