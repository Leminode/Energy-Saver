using Newtonsoft.Json;

namespace Energy_Saver.Model
{
    public class Taxes
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("year")]
        public int Year { get; set; }

        [JsonProperty("month")]
        public string Month { get; set; }

        [JsonProperty("gasPrice")]
        public float GasPrice { get; set; }

        [JsonProperty("electricityPrice")]
        public float ElectricityPrice { get; set; }

        [JsonProperty("waterPrice")]
        public float WaterPrice { get; set; }

        [JsonProperty("heatingPrice")]
        public float HeatingPrice { get; set; }
        
    }
}