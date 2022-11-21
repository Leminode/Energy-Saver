using Energy_Saver.DataSpace;
using Energy_Saver.Model;
using Energy_Saver.Services;
using Moq;

namespace Energy_Saver.Tests
{
    public class SuggestionsServiceTests
    {
        [Fact]
        public void GetLatestTaxComparison_NoTaxesExist_ReturnsEmptyList()
        {
            using (var db = new EnergySaverTaxesContext(Utilities.TestDbContextOptions()))
            {
                SuggestionsService suggestionsService = new SuggestionsService();

                List<decimal> expectedTaxList = new List<decimal>();
                List<decimal> actualTaxList = new List<decimal>();

                List<List<Taxes>> Taxes = new List<List<Taxes>>();

                var mock = new Mock<ISuggestionsService>();
                mock.Setup(s => s.GetLatestTaxComparison(Taxes)).Returns(() => new List<decimal> {});

                expectedTaxList = mock.Object.GetLatestTaxComparison(Taxes);
                actualTaxList = suggestionsService.GetLatestTaxComparison(Taxes);

                Assert.Equal(expectedTaxList, actualTaxList);
            }
        }

        [Fact]
        public void GetLatestTaxComparison_TwoTaxesExist_ReturnsPositivePercent()
        {
            using (var db = new EnergySaverTaxesContext(Utilities.TestDbContextOptions()))
            {
                SuggestionsService suggestionsService = new SuggestionsService();

                List<decimal> expectedTaxList = new List<decimal>();
                List<decimal> actualTaxList = new List<decimal>();

                List<List<Taxes>> Taxes = new List<List<Taxes>>();

                List<Taxes> taxList = new List<Taxes>()
                {
                    new Taxes
                    {
                        ID = 1,
                        UserID = 4,
                        Year = 2021,
                        GasAmount = 2,
                        ElectricityAmount = 2,
                        WaterAmount = 2,
                        HeatingAmount = 2
                    },
                    new Taxes
                    {
                        ID = 2,
                        UserID = 4,
                        Year = 2020,
                        GasAmount = 1,
                        ElectricityAmount = 1,
                        WaterAmount = 1,
                        HeatingAmount = 1
                    }
                };

                Taxes.Add(taxList);

                var mock = new Mock<ISuggestionsService>();
                mock.Setup(s => s.GetLatestTaxComparison(Taxes)).Returns(() => new List<decimal> { 100, 100, 100, 100 });

                expectedTaxList = mock.Object.GetLatestTaxComparison(Taxes);
                actualTaxList = suggestionsService.GetLatestTaxComparison(Taxes);

                Assert.Equal(expectedTaxList, actualTaxList);
            }
        }

        [Fact]
        public void GetLatestTaxComparison_TwoTaxesExist_ReturnsNegativePercent()
        {
            using (var db = new EnergySaverTaxesContext(Utilities.TestDbContextOptions()))
            {
                SuggestionsService suggestionsService = new SuggestionsService();

                List<decimal> expectedTaxList = new List<decimal>();
                List<decimal> actualTaxList = new List<decimal>();

                List<List<Taxes>> Taxes = new List<List<Taxes>>();

                List<Taxes> taxList = new List<Taxes>()
                {
                    new Taxes
                    {
                        ID = 1,
                        UserID = 4,
                        Year = 2020,
                        GasAmount = 1,
                        ElectricityAmount = 1,
                        WaterAmount = 1,
                        HeatingAmount = 1
                    },
                    new Taxes
                    {
                        ID = 2,
                        UserID = 4,
                        Year = 2021,
                        GasAmount = 2,
                        ElectricityAmount = 2,
                        WaterAmount = 2,
                        HeatingAmount = 2
                    }
                };

                Taxes.Add(taxList);

                var mock = new Mock<ISuggestionsService>();
                mock.Setup(s => s.GetLatestTaxComparison(Taxes)).Returns(() => new List<decimal> { -50, -50, -50, -50 });

                expectedTaxList = mock.Object.GetLatestTaxComparison(Taxes);
                actualTaxList = suggestionsService.GetLatestTaxComparison(Taxes);

                Assert.Equal(expectedTaxList, actualTaxList);
            }
        }

        [Fact]
        public void GetLatestTaxComparison_ThreeTaxesExist_ReturnsZero()
        {
            using (var db = new EnergySaverTaxesContext(Utilities.TestDbContextOptions()))
            {
                SuggestionsService suggestionsService = new SuggestionsService();

                List<decimal> expectedTaxList = new List<decimal>();
                List<decimal> actualTaxList = new List<decimal>();

                List<List<Taxes>> Taxes = new List<List<Taxes>>();

                List<Taxes> taxList = new List<Taxes>()
                {
                    new Taxes
                    {
                        ID = 1,
                        UserID = 4,
                        Year = 2020,
                        GasAmount = 2,
                        ElectricityAmount = 2,
                        WaterAmount = 2,
                        HeatingAmount = 2
                    },
                    new Taxes
                    {
                        ID = 2,
                        UserID = 4,
                        Year = 2021,
                        GasAmount = 2,
                        ElectricityAmount = 2,
                        WaterAmount = 2,
                        HeatingAmount = 2
                    },

                    new Taxes
                    {
                        ID = 3,
                        UserID = 4,
                        Year = 2022,
                        GasAmount = 1,
                        ElectricityAmount = 1,
                        WaterAmount = 1,
                        HeatingAmount = 1
                    }
                };

                Taxes.Add(taxList);

                var mock = new Mock<ISuggestionsService>();
                mock.Setup(s => s.GetLatestTaxComparison(Taxes)).Returns(() => new List<decimal> { 0, 0, 0, 0 });

                expectedTaxList = mock.Object.GetLatestTaxComparison(Taxes);
                actualTaxList = suggestionsService.GetLatestTaxComparison(Taxes);

                Assert.Equal(expectedTaxList, actualTaxList);
            }
        }
    }
}
