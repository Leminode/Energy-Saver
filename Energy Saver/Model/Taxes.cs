using Newtonsoft.Json;

namespace Energy_Saver.Model
{
    public class Taxes
    {
        public int Id { get; set; }

        public int Year { get; set; }

        public string Month { get; set; }

        public float GasPrice { get; set; }

        public float ElectricityPrice { get; set; }

        public float WaterPrice { get; set; }

        public float HeatingPrice { get; set; }
        
    }
}