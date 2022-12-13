using Energy_Saver.DataSpace;
using Energy_Saver.Model;
using Energy_Saver.Services;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using Xunit.Sdk;

namespace Energy_Saver.Tests
{
    public class SuggestionsServiceTests
    {
        [Fact]
        public void GetLatestTaxComparison_NoTaxesExist_ReturnsEmptyList()
        {
            SuggestionsService suggestionsService = new SuggestionsService();

            List<decimal> actualTaxList = new List<decimal>();

            actualTaxList = suggestionsService.GetLatestTaxComparison(new List<List<Taxes>>());

            actualTaxList.Should().BeEquivalentTo(new List<decimal> { });
        }

        [Theory]
        [InlineData(1, 0, 0, 0)]
        [InlineData(0, 1, 0, 0)]
        [InlineData(0, 0, 1, 0)]
        [InlineData(0, 0, 0, 1)]
        public void GetLatestTaxComparison_AnyTaxEqualsZero_ReturnsEmptyList(decimal gas, decimal electricity, decimal water, decimal heating)
        {
            SuggestionsService suggestionsService = new SuggestionsService();

            List<decimal> actualTaxList = new List<decimal>();

            List<List<Taxes>> Taxes = new List<List<Taxes>>()
            {
                new List<Taxes>()
                {
                    new Taxes
                    {
                        ID = 1,
                        UserID = 4,
                        Year = 2021,
                        GasAmount = gas,
                        ElectricityAmount = electricity,
                        WaterAmount = water,
                        HeatingAmount = heating
                    }
                }
            };

            actualTaxList = suggestionsService.GetLatestTaxComparison(Taxes);

            actualTaxList.Should().BeEquivalentTo(new List<decimal> { });
        }

        [Theory]
        [InlineData(4, 4, 4, 4, 2, 2, 2, 2)]
        [InlineData(2.2, 2.2, 2.2, 2.2, 1.1, 1.1, 1.1, 1.1)]
        [InlineData(4, 6, 8, 10, 2, 3, 4, 5)]
        [InlineData(78, 150, 44, 100, 39, 75, 22, 50)]
        public void GetLatestTaxComparison_TwoTaxesExist_ReturnsPositivePercent(decimal gas1, decimal electricity1, decimal water1, decimal heating1, decimal gas2, decimal electricity2, decimal water2, decimal heating2)
        {
            SuggestionsService suggestionsService = new SuggestionsService();

            List<decimal> actualTaxList = new List<decimal>();

            List<List<Taxes>> Taxes = new List<List<Taxes>>()
            {
                new List<Taxes>()
                {
                    new Taxes
                    {
                        ID = 1,
                        UserID = 4,
                        Year = 2021,
                        GasAmount = gas1,
                        ElectricityAmount = electricity1,
                        WaterAmount = water1,
                        HeatingAmount = heating1
                    },
                    new Taxes
                    {
                        ID = 2,
                        UserID = 4,
                        Year = 2020,
                        GasAmount = gas2,
                        ElectricityAmount = electricity2,
                        WaterAmount = water2,
                        HeatingAmount = heating2
                    }
                }
            };

            actualTaxList = suggestionsService.GetLatestTaxComparison(Taxes);

            actualTaxList.Should().BeEquivalentTo(new List<decimal> { 100, 100, 100, 100 });
        }

        [Theory]
        [InlineData(1, 1, 1, 1, 2, 2, 2, 2)]
        [InlineData(2.2, 3.3, 4.4, 5.5, 4.4, 6.6, 8.8, 11)]
        [InlineData(11, 22, 33, 44, 22, 44, 66, 88)]
        [InlineData(31, 47, 42, 50, 62, 94, 84, 100)]
        public void GetLatestTaxComparison_TwoTaxesExist_ReturnsNegativePercent(decimal gas1, decimal electricity1, decimal water1, decimal heating1, decimal gas2, decimal electricity2, decimal water2, decimal heating2)
        {
            SuggestionsService suggestionsService = new SuggestionsService();

            List<decimal> actualTaxList = new List<decimal>();

            List<List<Taxes>> Taxes = new List<List<Taxes>>()
            {
                new List<Taxes>()
                {
                    new Taxes
                    {
                        ID = 1,
                        UserID = 4,
                        Year = 2020,
                        GasAmount = gas1,
                        ElectricityAmount = electricity1,
                        WaterAmount = water1,
                        HeatingAmount = heating1
                    },
                    new Taxes
                    {
                        ID = 2,
                        UserID = 4,
                        Year = 2021,
                        GasAmount = gas2,
                        ElectricityAmount = electricity2,
                        WaterAmount = water2,
                        HeatingAmount = heating2
                    }
                }
            };

            actualTaxList = suggestionsService.GetLatestTaxComparison(Taxes);

            actualTaxList.Should().BeEquivalentTo(new List<decimal> { -50, -50, -50, -50 });
        }

        [Theory]
        [InlineData(1, 1, 1, 1)]
        [InlineData(700, 700, 700, 700)]
        [InlineData(5230, 5230, 5230, 5230)]
        [InlineData(142.2, 142.2, 142.2, 142.2)]
        public void GetLatestTaxComparison_ThreeTaxesExist_ReturnsZero(decimal gas, decimal electricity, decimal water, decimal heating)
        {
            SuggestionsService suggestionsService = new SuggestionsService();

            List<decimal> actualTaxList = new List<decimal>();

            List<List<Taxes>> Taxes = new List<List<Taxes>>()
            {
                new List<Taxes>()
                {
                    new Taxes
                    {
                        ID = 1,
                        UserID = 4,
                        Year = 2020,
                        GasAmount = gas,
                        ElectricityAmount = electricity,
                        WaterAmount = water,
                        HeatingAmount = heating
                    },
                    new Taxes
                    {
                        ID = 2,
                        UserID = 4,
                        Year = 2021,
                        GasAmount = gas,
                        ElectricityAmount = electricity,
                        WaterAmount = water,
                        HeatingAmount = heating
                    },

                    new Taxes
                    {
                        ID = 3,
                        UserID = 4,
                        Year = 2022,
                        GasAmount = 150,
                        ElectricityAmount = 333,
                        WaterAmount = 5302,
                        HeatingAmount = 3254
                    }
                }
            };

            actualTaxList = suggestionsService.GetLatestTaxComparison(Taxes);

            actualTaxList.Should().BeEquivalentTo(new List<decimal> { 0, 0, 0, 0, });
        }

        [Fact]
        public void CheckForZerosTest_TwoTaxesNotZero_ReturnsTrue()
        {
            SuggestionsService suggestionsService = new SuggestionsService(); ;

            List<Taxes> list = new List<Taxes>()
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

            Assert.True(suggestionsService.CheckForZeros(list));
        }

        [Theory]
        [InlineData(1, 0, 0, 0)]
        [InlineData(0, 1, 0, 0)]
        [InlineData(0, 0, 1, 0)]
        [InlineData(0, 0, 0, 1)]
        public void CheckForZerosTest_TwoTaxZero_ReturnsFalse(decimal gas, decimal electricity, decimal water, decimal heating)
        {
            SuggestionsService suggestionsService = new SuggestionsService(); ;

            List<Taxes> list = new List<Taxes>()
            {
                new Taxes
                {
                    GasAmount = 100,
                    ElectricityAmount = 90.2M,
                    WaterAmount = 23,
                    HeatingAmount = 0
                },
                new Taxes
                {
                    GasAmount = gas,
                    ElectricityAmount = electricity,
                    WaterAmount = water,
                    HeatingAmount = heating
                }
            };

            Assert.False(suggestionsService.CheckForZeros(list));
        }

        [Theory]
        [InlineData(1, 1, 1, 1)]
        [InlineData(300, 300, 300, 300)]
        [InlineData(0, 0, 0, 0)]
        [InlineData(4.3, 4.3, 4.3, 4.3)]
        public void PercentageAboveOrBelowAverageTest_TwoTaxesExist_ReturnsZero(decimal gas, decimal electricity, decimal water, decimal heating)
        {
            SuggestionsService suggestionsService = new SuggestionsService();

            List<ISuggestionsService.TaxesWithSum> expectedData = new List<ISuggestionsService.TaxesWithSum>()
            {
                new ISuggestionsService.TaxesWithSum()
                {
                    Year = 2000,
                    Month = Months.January,
                    Percentage = 0,
                    Style = "text-neutral",
                    Icon = "bi-arrow-down-up"
                },

                new ISuggestionsService.TaxesWithSum()
                {
                    Year = 2000,
                    Month = Months.February,
                    Percentage = 0,
                    Style = "text-neutral",
                    Icon = "bi-arrow-down-up"
                }
            };

            List<List<Taxes>> taxList = new List<List<Taxes>>()
            {
                new List<Taxes>()
                {
                    new Taxes()
                    {
                        Year = 2000,
                        Month = Months.January,
                        GasAmount = gas,
                        ElectricityAmount = electricity,
                        WaterAmount = water,
                        HeatingAmount = heating
                    },
                    new Taxes()
                    {
                        Year = 2000,
                        Month = Months.February,
                        GasAmount = gas,
                        ElectricityAmount = electricity,
                        WaterAmount = water,
                        HeatingAmount = heating
                    }
                }
            };

            List<ISuggestionsService.TaxesWithSum> actualData = suggestionsService.PercetangeAboveOrBelowAverage(taxList);

            actualData.Should().BeEquivalentTo(expectedData, d => d.ComparingByMembers<ISuggestionsService.TaxesWithSum>());
        }

        [Theory]
        [InlineData(2, 2, 2, 2, 0, 0, 0, 0)]
        [InlineData(300, 300, 300, 300, 0, 0, 0, 0)]
        [InlineData(1, 1, 1, 1, 0, 0, 0, 0)]
        [InlineData(4.3, 4.3, 4.3, 4.3, 0, 0, 0, 0)]
        public void PercentageAboveOrBelowAverageTest_TwoTaxesExist_ReturnsOneHundredPercent(decimal gas1, decimal electricity1, decimal water1, decimal heating1, decimal gas2, decimal electricity2, decimal water2, decimal heating2)
        {
            SuggestionsService suggestionsService = new SuggestionsService();

            List<ISuggestionsService.TaxesWithSum> expectedData = new List<ISuggestionsService.TaxesWithSum>()
            {
                new ISuggestionsService.TaxesWithSum()
                {
                    Year = 2000,
                    Month = Months.January,
                    Percentage = -100,
                    Style = "text-positive",
                    Icon = "bi-arrow-down"
                },

                new ISuggestionsService.TaxesWithSum()
                {
                    Year = 2000,
                    Month = Months.February,
                    Percentage = 100,
                    Style = "text-negative",
                    Icon = "bi-arrow-up"
                }
            };

            List<List<Taxes>> taxList = new List<List<Taxes>>()
            {
                new List<Taxes>()
                {
                    new Taxes()
                    {
                        Year = 2000,
                        Month = Months.January,
                        GasAmount = gas2,
                        ElectricityAmount = electricity2,
                        WaterAmount = water2,
                        HeatingAmount = heating2
                    },
                    new Taxes()
                    {
                        Year = 2000,
                        Month = Months.February,
                        GasAmount = gas1,
                        ElectricityAmount = electricity1,
                        WaterAmount = water1,
                        HeatingAmount = heating1
                    }
                }
            };

            List<ISuggestionsService.TaxesWithSum> actualData = suggestionsService.PercetangeAboveOrBelowAverage(taxList);

            actualData.Should().BeEquivalentTo(expectedData, d => d.ComparingByMembers<ISuggestionsService.TaxesWithSum>());
        }

        [Theory]
        [InlineData(2, 2, 2, 2, 1, 1, 1, 1)]
        [InlineData(300, 300, 300, 300, 150, 150, 150, 150)]
        [InlineData(6, 6, 6, 6, 3, 3, 3, 3)]
        [InlineData(4.2, 4.2, 4.2, 4.2, 2.1, 2.1, 2.1, 2.1)]
        public void PercentageAboveOrBelowAverageTest_TwoTaxesExist_ReturnsNegativeAndPositivePercent(decimal gas1, decimal electricity1, decimal water1, decimal heating1, decimal gas2, decimal electricity2, decimal water2, decimal heating2)
        {
            SuggestionsService suggestionsService = new SuggestionsService();

            List<ISuggestionsService.TaxesWithSum> expectedData = new List<ISuggestionsService.TaxesWithSum>()
            {
                new ISuggestionsService.TaxesWithSum()
                {
                    Year = 2000,
                    Month = Months.January,
                    Percentage = -33.33M,
                    Style = "text-positive",
                    Icon = "bi-arrow-down"
                },

                new ISuggestionsService.TaxesWithSum()
                {
                    Year = 2000,
                    Month = Months.February,
                    Percentage = 33.33M,
                    Style = "text-negative",
                    Icon = "bi-arrow-up"
                }
            };

            List<List<Taxes>> taxList = new List<List<Taxes>>()
            {
                new List<Taxes>()
                {
                    new Taxes()
                    {
                        Year = 2000,
                        Month = Months.January,
                        GasAmount = gas2,
                        ElectricityAmount = electricity2,
                        WaterAmount = water2,
                        HeatingAmount = heating2
                    },
                    new Taxes()
                    {
                        Year = 2000,
                        Month = Months.February,
                        GasAmount = gas1,
                        ElectricityAmount = electricity1,
                        WaterAmount = water1,
                        HeatingAmount = heating1
                    }
                }
            };

            List<ISuggestionsService.TaxesWithSum> actualData = suggestionsService.PercetangeAboveOrBelowAverage(taxList);

            actualData.First().Should().BeEquivalentTo(expectedData.First(), d => d.ComparingByMembers<ISuggestionsService.TaxesWithSum>());

            actualData.Last().Should().BeEquivalentTo(expectedData.Last(), d => d.ComparingByMembers<ISuggestionsService.TaxesWithSum>());
        }
    }
}
