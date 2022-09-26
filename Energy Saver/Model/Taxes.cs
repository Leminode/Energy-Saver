using Microsoft.Build.Framework;
using Newtonsoft.Json;

namespace Energy_Saver.Model
{
    public class Taxes : IComparable<Taxes>
    {
        //public int Id { get; set; }

        public int Year { get; set; }

        public Months Month { get; set; }
        
        public decimal GasAmount { get; set; }

        public decimal ElectricityAmount { get; set; }

        public decimal WaterAmount { get; set; }

        public decimal HeatingAmount { get; set; }

        public int CompareTo(Taxes other)
        {
            if (this.Year == other.Year)
                return 0;
            else if (this.Year < other.Year)
                return -1;
            else
                return 1;
        }
    }
}