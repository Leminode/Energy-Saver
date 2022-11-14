using Energy_Saver.DataSpace;
using Energy_Saver.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace Energy_Saver.Tests
{
    public class UnitTests
    {
        [Fact]
        public async void TaxInputTest()
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

                List<Taxes> taxList = new List<Taxes>();
                taxList.Add(taxes);

                db.Taxes.Add(taxes);
                await db.SaveChangesAsync();

                var list = await db.Taxes.ToListAsync();

                Assert.Equal(taxList, list);
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
        public async void test()
        {
            using (var db = new EnergySaverTaxesContext(Utilities.TestDbContextOptions()))
            {
                var tax1 = new Taxes
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

                var tax2 = new Taxes
                {
                    ID = 2,
                    UserID = 4,
                    Year = 2020,
                    Month = Months.February,
                    GasAmount = 2,
                    ElectricityAmount = 4,
                    WaterAmount = 3,
                    HeatingAmount = 2
                };

                db.Taxes.Add(taxes);
                await db.SaveChangesAsync();

                db.Taxes.Remove(taxes);
                await db.SaveChangesAsync();

                Assert.False(db.Taxes.Any());
            }
        }
    }
}