using Energy_Saver.Model;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace Energy_Saver.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var taxes = new Taxes
            {
                ID = 1,
                UserID = 4,
                Month = Months.January,
                GasAmount = 1,
                ElectricityAmount = 2,
                WaterAmount = 3,
                HeatingAmount = 4
            };

            
        }
    }
}