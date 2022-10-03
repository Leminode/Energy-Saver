using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Energy_Saver.Model
{
    public class Taxes : IComparable<Taxes>
    {
        //public int Id { get; set; }

        public int Year { get; set; }

        public Months Month { get; set; }

        [RegularExpression(@"^(\d *\.)?\d+$", ErrorMessage = "Invalid input")]
        [Required(ErrorMessage = "Gas amount is required")]
        public decimal GasAmount { get; set; }

        [RegularExpression(@"^(\d *\.)?\d+$", ErrorMessage = "Invalid input")]
        [Required(ErrorMessage = "Electricity amount is required")]
        public decimal ElectricityAmount { get; set; }

        [RegularExpression(@"^(\d *\.)?\d+$", ErrorMessage = "Invalid input")]
        [Required(ErrorMessage = "Water amount is required")]
        public decimal WaterAmount { get; set; }

        [RegularExpression(@"^(\d *\.)?\d+$", ErrorMessage = "Invalid input")]
        [Required(ErrorMessage = "Heating amount is required")]
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