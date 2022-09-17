namespace Energy_Saver.Model
{
    public class Taxes
    {
        public int Id { get; set; }
        public string Month { get; set; }
        public int GasPrice { get; set; }
        public int ElectricityPrice { get; set; }
        public int WaterPrice { get; set; }
        public int HeatingPrice { get; set; }

    }
}