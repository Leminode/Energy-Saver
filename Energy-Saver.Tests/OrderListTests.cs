using Energy_Saver.DataSpace;
using Energy_Saver.Model;
using static Energy_Saver.Model.Serialization;

namespace Energy_Saver.Tests
{
    public class OrderListTests
    {
        [Fact]
        public void OrderList_TwoTaxesExist_ReturnsAscendingSortedListByYear()
        {
            using (var db = new EnergySaverTaxesContext(Utilities.TestDbContextOptions()))
            {
                List<Taxes> actualTaxList = new List<Taxes>();
                List<Taxes> expectedTaxList = new List<Taxes>();

                Taxes tax1 = new Taxes
                {
                    Year = 2020,
                };
                Taxes tax2 = new Taxes
                {
                    Year = 2021,
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
        public void OrderList_TwoTaxesExist_ReturnsDescendingSortedListByYear()
        {
            using (var db = new EnergySaverTaxesContext(Utilities.TestDbContextOptions()))
            {
                List<Taxes> actualTaxList = new List<Taxes>();
                List<Taxes> expectedTaxList = new List<Taxes>();

                Taxes tax1 = new Taxes
                {
                    Year = 2020,
                };
                Taxes tax2 = new Taxes
                {
                    Year = 2021,
                };

                actualTaxList.Add(tax1);
                actualTaxList.Add(tax2);

                expectedTaxList.Add(tax2);
                expectedTaxList.Add(tax1);

                actualTaxList = OrderList(SortDirection.Descending, actualTaxList, tax => tax.Year);

                Assert.Equal(expectedTaxList, actualTaxList);
            }
        }

        [Fact]
        public void OrderList_FourTaxesExist_ReturnsDescendingSortedListByMonth()
        {
            using (var db = new EnergySaverTaxesContext(Utilities.TestDbContextOptions()))
            {
                List<Taxes> actualTaxList = new List<Taxes>();
                List<Taxes> expectedTaxList = new List<Taxes>();

                Taxes[] taxList = new Taxes[]
                {
                    new Taxes
                    {
                        Year = 2020,
                        Month = Months.January,
                    },

                    new Taxes
                    {
                        Year = 2020,
                        Month = Months.April,
                    },

                    new Taxes
                    {
                        Year = 2020,
                        Month = Months.February,
                    },

                    new Taxes
                    {
                        Year = 2020,
                        Month = Months.July,
                    }
                };

                for(int i = 0; i < taxList.Length; i++)
                {
                    actualTaxList.Add(taxList[i]);
                }

                expectedTaxList.Add(taxList[3]);
                expectedTaxList.Add(taxList[1]);
                expectedTaxList.Add(taxList[2]);
                expectedTaxList.Add(taxList[0]);

                actualTaxList = OrderList(SortDirection.Descending, actualTaxList, tax => tax.Month);

                Assert.Equal(expectedTaxList, actualTaxList);
            }
        }

        [Fact]
        public void OrderList_FiveTaxesExist_ReturnsAscendingSortedListByMonth()
        {
            using (var db = new EnergySaverTaxesContext(Utilities.TestDbContextOptions()))
            {
                List<Taxes> actualTaxList = new List<Taxes>();
                List<Taxes> expectedTaxList = new List<Taxes>();

                Taxes[] taxList = new Taxes[]
                {
                    new Taxes
                    {
                        Year = 2020,
                        Month = Months.August,
                    },

                    new Taxes
                    {
                        Year = 2020,
                        Month = Months.April,
                    },

                    new Taxes
                    {
                        Year = 2020,
                        Month = Months.February,
                    },

                    new Taxes
                    {
                        Year = 2020,
                        Month = Months.July,
                    },

                    new Taxes
                    {
                        Year = 2020,
                        Month = Months.January,
                    }
                };

                for (int i = 0; i < taxList.Length; i++)
                {
                    actualTaxList.Add(taxList[i]);
                }

                expectedTaxList.Add(taxList[4]);
                expectedTaxList.Add(taxList[2]);
                expectedTaxList.Add(taxList[1]);
                expectedTaxList.Add(taxList[3]);
                expectedTaxList.Add(taxList[0]);

                actualTaxList = OrderList(SortDirection.Ascending, actualTaxList, taxes => taxes.Month);

                Assert.Equal(expectedTaxList, actualTaxList);
            }
        }
    }
}