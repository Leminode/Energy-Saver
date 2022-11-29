using Energy_Saver.DataSpace;
using Energy_Saver.Model;
using Newtonsoft.Json;
using static Energy_Saver.Model.Utilities;

namespace Energy_Saver.Tests
{
    public class UtilitiesTests
    {
        [Theory]
        [InlineData(2020, 2021)]
        [InlineData(0, 1)]
        [InlineData(7, 10023)]
        [InlineData(1, 3)]
        public void OrderList_TwoTaxesExist_ReturnsAscendingSortedListByYear(int year1, int year2)
        {
            List<Taxes> actualTaxList = new List<Taxes>();
            List<Taxes> expectedTaxList = new List<Taxes>();

            Taxes tax1 = new Taxes
            {
                Year = year1,
            };
            Taxes tax2 = new Taxes
            {
                Year = year2,
            };

            actualTaxList.Add(tax2);
            actualTaxList.Add(tax1);

            expectedTaxList.Add(tax1);
            expectedTaxList.Add(tax2);

            actualTaxList = OrderList(SortDirection.Ascending, actualTaxList, tax => tax.Year);

            var expectedTaxListString = JsonConvert.SerializeObject(expectedTaxList);
            var actualTaxListString = JsonConvert.SerializeObject(actualTaxList);

            Assert.Equal(expectedTaxListString, actualTaxListString);
        }

        [Theory]
        [InlineData(2020, 2021)]
        [InlineData(0, 1)]
        [InlineData(7, 10023)]
        [InlineData(1, 3)]
        public void OrderList_TwoTaxesExist_ReturnsDescendingSortedListByYear(int year1, int year2)
        {
            List<Taxes> actualTaxList = new List<Taxes>();
            List<Taxes> expectedTaxList = new List<Taxes>();

            Taxes tax1 = new Taxes
            {
                Year = year1,
            };
            Taxes tax2 = new Taxes
            {
                Year = year2,
            };

            actualTaxList.Add(tax1);
            actualTaxList.Add(tax2);

            expectedTaxList.Add(tax2);
            expectedTaxList.Add(tax1);

            actualTaxList = OrderList(SortDirection.Descending, actualTaxList, tax => tax.Year);

            var expectedTaxListString = JsonConvert.SerializeObject(expectedTaxList);
            var actualTaxListString = JsonConvert.SerializeObject(actualTaxList);

            Assert.Equal(expectedTaxListString, actualTaxListString);
        }

        [Theory]
        [InlineData(Months.January, Months.March, Months.February, Months.April)]
        [InlineData(Months.February, Months.November, Months.October, Months.December)]
        [InlineData(Months.April, Months.August, Months.June, Months.October)]
        [InlineData(Months.June, Months.September, Months.July, Months.November)]
        public void OrderList_FourTaxesExist_ReturnsDescendingSortedListByMonth(Months month1, Months month2, Months month3, Months month4)
        {
            List<Taxes> actualTaxList = new List<Taxes>();
            List<Taxes> expectedTaxList = new List<Taxes>();

            Taxes[] taxList = new Taxes[]
            {
                new Taxes
                {
                    Year = 2020,
                    Month = month1,
                },

                new Taxes
                {
                    Year = 2020,
                    Month = month2,
                },

                new Taxes
                {
                    Year = 2020,
                    Month = month3,
                },

                new Taxes
                {
                    Year = 2020,
                    Month = month4,
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

            var expectedTaxListString = JsonConvert.SerializeObject(expectedTaxList);
            var actualTaxListString = JsonConvert.SerializeObject(actualTaxList);

            Assert.Equal(expectedTaxListString, actualTaxListString);
        }

        [Theory]
        [InlineData(Months.May, Months.April, Months.March, Months.February, Months.January)]
        [InlineData(Months.December, Months.November, Months.October, Months.September, Months.August)]
        [InlineData(Months.November, Months.August, Months.June, Months.April, Months.January)]
        [InlineData(Months.December, Months.September, Months.July, Months.April, Months.March)]
        public void OrderList_FiveTaxesExist_ReturnsAscendingSortedListByMonth(Months month1, Months month2, Months month3, Months month4, Months month5)
        {
            List<Taxes> actualTaxList = new List<Taxes>();
            List<Taxes> expectedTaxList = new List<Taxes>();

            Taxes[] taxList = new Taxes[]
            {
                new Taxes
                {
                    Year = 2020,
                    Month = month1,
                },

                new Taxes
                {
                    Year = 2020,
                    Month = month2,
                },

                new Taxes
                {
                    Year = 2020,
                    Month = month3,
                },

                new Taxes
                {
                    Year = 2020,
                    Month = month4,
                },

                new Taxes
                {
                    Year = 2020,
                    Month = month5,
                }
            };

            for (int i = 0; i < taxList.Length; i++)
            {
                actualTaxList.Add(taxList[i]);
            }

            expectedTaxList.Add(taxList[4]);
            expectedTaxList.Add(taxList[3]);
            expectedTaxList.Add(taxList[2]);
            expectedTaxList.Add(taxList[1]);
            expectedTaxList.Add(taxList[0]);

            actualTaxList = OrderList(SortDirection.Ascending, actualTaxList, taxes => taxes.Month);

            var expectedTaxListString = JsonConvert.SerializeObject(expectedTaxList);
            var actualTaxListString = JsonConvert.SerializeObject(actualTaxList);

            Assert.Equal(expectedTaxListString, actualTaxListString);
        }

        [Fact]
        public void FormatMonthTest()
        {
            foreach(Months month in Enum.GetValues(typeof(Months)))
            {
                Assert.Equal(((int)month).ToString().PadLeft(2, '0'), FormatMonth(month));
            }
        }
    }
}