using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Energy_Saver.Model
{
    [Table("taxes")]
    public class Taxes : IComparable<Taxes>
    {
        [Column("id")]
        public int ID { get; set; }

        [Column("user_id")]
        public int user_ID { get; set; }

        [Column("year")]
        public int Year { get; set; }

        [Column("month")]
        public Months Month { get; set; }

        [RegularExpression(@"\d+(.\d+)?$", ErrorMessage = "Invalid input")]
        [Required(ErrorMessage = "Gas amount is required")]
        [Column("gas_amount")]
        public decimal GasAmount { get; set; }

        [RegularExpression(@"\d+(.\d+)?$$", ErrorMessage = "Invalid input")]
        [Required(ErrorMessage = "Electricity amount is required")]
        [Column("electricity_amount")]
        public decimal ElectricityAmount { get; set; }

        [RegularExpression(@"\d+(.\d+)?$", ErrorMessage = "Invalid input")]
        [Required(ErrorMessage = "Water amount is required")]
        [Column("water_amount")]
        public decimal WaterAmount { get; set; }

        [RegularExpression(@"\d+(.\d+)?$", ErrorMessage = "Invalid input")]
        [Required(ErrorMessage = "Heating amount is required")]
        [Column("heating_amount")]
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