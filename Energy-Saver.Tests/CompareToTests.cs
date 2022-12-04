using Energy_Saver.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Energy_Saver.Tests
{
    public class CompareToTests
    {
        [Theory]
        [InlineData(2000, 2020)]
        [InlineData(1, 2)]
        [InlineData(-123, 0)]
        [InlineData(12315, 1111111)]
        public void CompareToTest_TwoTaxesExist_ReturnsNegativeOne(int year1, int year2)
        {
            Taxes tax1 = new Taxes()
            {
                Year = year1
            };

            Taxes tax2 = new Taxes()
            {
                Year = year2
            };

            int comparison = tax1.CompareTo(tax2);

            Assert.True(comparison == -1);
        }
        [Theory]
        [InlineData(2000, 2020)]
        [InlineData(1, 2)]
        [InlineData(-123, 0)]
        [InlineData(12315, 1111111)]
        public void CompareToTest_TwoTaxesExist_ReturnsPositive(int year1, int year2)
        {
            Taxes tax1 = new Taxes()
            {
                Year = year2
            };

            Taxes tax2 = new Taxes()
            {
                Year = year1
            };

            int comparison = tax1.CompareTo(tax2);

            Assert.True(comparison == 1);
        }

        [Fact]
        public void CompareToTest_TwoTaxesExist_ReturnsZero()
        {
            Taxes tax1 = new Taxes()
            {
                Year = 2000
            };

            Taxes tax2 = new Taxes()
            {
                Year = 2000
            };

            int comparison = tax1.CompareTo(tax2);

            Assert.True(comparison == 0);
        }

    }
}
