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

                List<string> expectedMonths = new List<string>()
                {
                    "January",
                    "February",
                    "March",
                    "April",
                    "May",
                    "June",
                    "July",
                    "August",
                    "September",
                    "October",
                    "November",
                    "December"
                };

                var actualMonths = statisticsModel.GenerateMonthsLabels();

                Assert.Equal(expectedMonths, actualMonths);
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

                List<string> expectedTaxLabels = new List<string>()
                {
                    "Gas",
                    "Electricity",
                    "Water",
                    "Heating"
                };

                var actualTaxLabels = statisticsModel.GenerateTaxLabels();

                Assert.Equal(expectedTaxLabels, actualTaxLabels);
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

                List<double?> data = new List<double?> { 4, 3, 2, 0, 0, 0, 1, 0, 0, 0, 0, 0 };

                List<ChartService.DataWithLabel> expectedData = new List<ChartService.DataWithLabel>()
                {
                    new ChartService.DataWithLabel
                    {
                        Label = ChartService.FilterTypes.Gas.ToString(),
                        Data = data
                    },

                    new ChartService.DataWithLabel
                    {
                        Label = ChartService.FilterTypes.Electricity.ToString(),
                        Data = data
                    },

                    new ChartService.DataWithLabel
                    {
                        Label = ChartService.FilterTypes.Water.ToString(),
                        Data = data
                    },

                    new ChartService.DataWithLabel
                    {
                        Label = ChartService.FilterTypes.Heating.ToString(),
                        Data = data
                    }
                };

                statisticsModel.Taxes = new List<List<Taxes>>()
                {
                    new List<Taxes>()
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
                    }
                };

                List<ChartService.DataWithLabel> actualData = statisticsModel.CreateDataForYearChart(2020);

                actualData.Should().BeEquivalentTo(expectedData, d => d.ComparingByMembers<ChartService.DataWithLabel>());
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
                        Label = month.ToString(),
                        Data = new List<double?> { (double)gas, (double)electricity, (double)water, (double)heating }
                    }
                };

                statisticsModel.Taxes = new List<List<Taxes>>()
                {
                    new List<Taxes>()
                    {
                        new Taxes
                        {
                            Year = 2020,
                            Month = month,
                            GasAmount = gas,
                            ElectricityAmount = electricity,
                            WaterAmount = water,
                            HeatingAmount = heating
                        }
                    }
                };

                List<ChartService.DataWithLabel> actualData = statisticsModel.CreateDataForMonthChart(month, 2020);

                actualData.Should().BeEquivalentTo(expectedData, d => d.ComparingByMembers<ChartService.DataWithLabel>());
            }
        }

        [Fact]
        public void GetRandomChartColorTest()
        {
            int actualColor;
            ChartColor chartColor;

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
    }
}
