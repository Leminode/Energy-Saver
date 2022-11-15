using Energy_Saver.DataSpace;
using Energy_Saver.Model;
using Microsoft.EntityFrameworkCore;
using static Energy_Saver.Model.Serialization;

namespace Energy_Saver.Tests
{
    public class UnitTests// : IClassFixture<IndexModel>
    {
        //private readonly ILogger<IndexModel> _logger;
        //private readonly ISuggestionsService _suggestionsService;
        //private readonly EnergySaverTaxesContext _context;

        //public UnitTests(ILogger<IndexModel> logger, ISuggestionsService suggestionsService, EnergySaverTaxesContext context)
        //{
        //    _logger = logger;
        //    _suggestionsService = suggestionsService;
        //    _context = context;
        //}

            [Fact]
        public async void TaxInputTest()
        {
            using (var db = new EnergySaverTaxesContext(Utilities.TestDbContextOptions()))
            {
                List<Taxes> expectedTaxList = new List<Taxes>();

                var taxes = new Taxes[]
                {
                    new Taxes
                    {
                        ID = 1,
                        UserID = 4,
                        Year = 2020,
                        Month = Months.January,
                        GasAmount = 1,
                        ElectricityAmount = 2,
                        WaterAmount = 3,
                        HeatingAmount = 4
                    },
                    new Taxes
                    {
                        ID = 2,
                        UserID = 4,
                        Year = 2021,
                        Month = Months.February,
                        GasAmount = 2,
                        ElectricityAmount = 4,
                        WaterAmount = 3,
                        HeatingAmount = 2
                    },
                    new Taxes
                    {
                        ID = 3,
                        UserID = 4,
                        Year = 2022,
                        Month = Months.June,
                        GasAmount = 90,
                        ElectricityAmount = 60.4M,
                        WaterAmount = 51.4M,
                        HeatingAmount = 36
                    }
                };

                for(int i = 0; i < taxes.Length; i++)
                {
                    expectedTaxList.Add(taxes[i]);
                    db.Taxes.Add(taxes[i]);
                }

                await db.SaveChangesAsync();

                var actualTaxList = await db.Taxes.ToListAsync();

                Assert.Equal(expectedTaxList, actualTaxList);
            }
        }

        [Fact]
        public async void TaxRemoveTest()
        {
            using (var db = new EnergySaverTaxesContext(Utilities.TestDbContextOptions()))
            {
                var taxes = new Taxes
                {
                    ID = 1,
                    UserID = 4,
                    Year = 2020,
                    Month = Months.January,
                    GasAmount = 1,
                    ElectricityAmount = 2,
                    WaterAmount = 3,
                    HeatingAmount = 4
                };

                db.Taxes.Add(taxes);
                await db.SaveChangesAsync();

                db.Taxes.Remove(taxes);
                await db.SaveChangesAsync();

                Assert.False(db.Taxes.Any());
            }
        }

        [Fact]
        public async void AscendingOrderListTest()
        {
            using (var db = new EnergySaverTaxesContext(Utilities.TestDbContextOptions()))
            {
                List<Taxes> actualTaxList = new List<Taxes>();
                List<Taxes> expectedTaxList = new List<Taxes>();

                Taxes tax1 = new Taxes
                {
                    ID = 1,
                    UserID = 4,
                    Year = 2020,
                    Month = Months.January,
                    GasAmount = 1,
                    ElectricityAmount = 2,
                    WaterAmount = 3,
                    HeatingAmount = 4
                };
                Taxes tax2 = new Taxes
                {
                    ID = 2,
                    UserID = 4,
                    Year = 2021,
                    Month = Months.February,
                    GasAmount = 2,
                    ElectricityAmount = 4,
                    WaterAmount = 3,
                    HeatingAmount = 2
                };

                actualTaxList.Add(tax2);
                actualTaxList.Add(tax1);

                expectedTaxList.Add(tax1);
                expectedTaxList.Add(tax2);

                actualTaxList = OrderList(SortDirection.Ascending, actualTaxList, tax => tax.Year);

                Assert.Equal(expectedTaxList, actualTaxList);
            }
        }

        [Fact]
        public void DescendingOrderListTest()
        {
            using (var db = new EnergySaverTaxesContext(Utilities.TestDbContextOptions()))
            {
                List<Taxes> actualTaxList = new List<Taxes>();
                List<Taxes> expectedTaxList = new List<Taxes>();

                Taxes tax1 = new Taxes
                {
                    ID = 1,
                    UserID = 4,
                    Year = 2020,
                    Month = Months.January,
                    GasAmount = 1,
                    ElectricityAmount = 2,
                    WaterAmount = 3,
                    HeatingAmount = 4
                };
                Taxes tax2 = new Taxes
                {
                    ID = 2,
                    UserID = 4,
                    Year = 2021,
                    Month = Months.February,
                    GasAmount = 2,
                    ElectricityAmount = 4,
                    WaterAmount = 3,
                    HeatingAmount = 2
                };

                actualTaxList.Add(tax1);
                actualTaxList.Add(tax2);

                expectedTaxList.Add(tax2);
                expectedTaxList.Add(tax1);

                actualTaxList = OrderList(SortDirection.Descending, actualTaxList, tax => tax.Year);

                Assert.Equal(expectedTaxList, actualTaxList);
            }
        }

        //[Fact]
        //public void GetLatestTaxComparisonTest()
        //{
        //    using (var db = new EnergySaverTaxesContext(Utilities.TestDbContextOptions()))
        //    {
        //        List<List<Taxes>> taxList = new List<List<Taxes>>();
        //        List<Taxes> taxes = new List<Taxes>();

        //        List<decimal> actualTaxList = new List<decimal>();
        //        List<decimal> expectedTaxList = new List<decimal>();

        //        var tax1 = new Taxes
        //        {
        //            ID = 1,
        //            UserID = 4,
        //            Year = 2020,
        //            Month = Months.January,
        //            GasAmount = 1,
        //            ElectricityAmount = 2,
        //            WaterAmount = 3,
        //            HeatingAmount = 4
        //        };

        //        var tax2 = new Taxes
        //        {
        //            ID = 2,
        //            UserID = 4,
        //            Year = 2020,
        //            Month = Months.February,
        //            GasAmount = 2,
        //            ElectricityAmount = 4,
        //            WaterAmount = 3,
        //            HeatingAmount = 2
        //        };

        //        taxes.Add(tax1);
        //        taxes.Add(tax2);

        //        taxList.Add(taxes);

        //        actualTaxList = _suggestionsService.GetLatestTaxComparison(taxList);

        //        //expectedTaxList

        //        Assert.Equal(expectedTaxList, actualTaxList);
        //    }
        //}
    }
}