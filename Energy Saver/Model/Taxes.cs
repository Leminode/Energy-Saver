using Newtonsoft.Json;

namespace Energy_Saver.Model
{
    public class Taxes
    {
        public int Id { get; set; }

        public int Year { get; set; }

        public string Month { get; set; }

        /*public enum Month
        {
            January,
            February,
            March,
            April,
            May,
            June,
            July,
            August,
            September,
            October,
            November,
            December,
        }*/

        public float GasAmount { get; set; }

        public float ElectricityAmount { get; set; }

        public float WaterAmount { get; set; }

        public float HeatingAmount { get; set; }
        
    }
}