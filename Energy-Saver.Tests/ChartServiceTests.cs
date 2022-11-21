using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Energy_Saver.DataSpace;
using Energy_Saver.Pages;
using Energy_Saver.Services;
using Energy_Saver.Model;

namespace Energy_Saver.Tests
{
    public class ChartServiceTests
    {
        [Fact]
        public void GetTaxesFromDatabaseTest()
        {
            using (var db = new EnergySaverTaxesContext(Utilities.TestDbContextOptions()))
            {
                ChartService chartService = new ChartService();

                StatisticsModel mod = new StatisticsModel(chartService, db);

                List<List<Taxes>> tax = new List<List<Taxes>>();

                tax = mod.GetTaxesFromDatabase();

                Assert.Equal(new List<List<Taxes>>{ }, tax);
            }
            
        }

        [Fact]
        public void GenerateMonthsLabelsTest()
        {
            using (var db = new EnergySaverTaxesContext(Utilities.TestDbContextOptions()))
            {
                ChartService chartService = new ChartService();

                List<string> expectedMonths = new List<string>();

                StatisticsModel mod = new StatisticsModel(chartService, db);
                var actualMonths = mod.GenerateMonthsLabels();

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

                List<string> expectedMonths = new List<string>();

                StatisticsModel mod = new StatisticsModel(chartService, db);
                var actualMonths = mod.GenerateTaxLabels();

                foreach (ChartService.FilterTypes filterTypes in Enum.GetValues(typeof(ChartService.FilterTypes)))
                {
                    Assert.Equal(filterTypes.ToString(), actualMonths.ElementAt((int)filterTypes));
                }
            }
        }

        [Fact]
        public void CreateDataForYearChartTest()
        {
            using (var db = new EnergySaverTaxesContext(Utilities.TestDbContextOptions()))
            {
                Taxes[] taxList = new Taxes[]
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
                ChartService chartService = new ChartService();

                List<ChartService.DataWithLabel> expectedMonths = new List<ChartService.DataWithLabel>();



                StatisticsModel mod = new StatisticsModel(chartService, db);
                var actualMonths = mod.CreateDataForYearChart(2022);

                

                //Assert.Equal(expectedMonths, actualMonths);
            }
        }
    }
}
