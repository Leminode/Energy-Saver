using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Energy_Saver.Model
{
    public class Taxes : IComparable<Taxes>, IValidatableObject
    {
        //public int Id { get; set; }

        public int Year { get; set; }

        public Months Month { get; set; }
        
        public decimal GasAmount { get; set; }

        public decimal ElectricityAmount { get; set; }

        public decimal WaterAmount { get; set; }

        public decimal HeatingAmount { get; set; }

        [Required]
        public bool Enable { get; set; }

        public int CompareTo(Taxes other)
        {
            if (this.Year == other.Year)
                return 0;
            else if (this.Year < other.Year)
                return -1;
            else
                return 1;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(this.GasAmount, new ValidationContext(this, null, null) { MemberName = "GasAmount" }, results);

            Validator.TryValidateObject(this.ElectricityAmount, new ValidationContext(this, null, null) { MemberName = "ElectricityAmount" }, results);

            Validator.TryValidateObject(this.WaterAmount, new ValidationContext(this, null, null) { MemberName = "WaterAmount" }, results);

            Validator.TryValidateObject(this.HeatingAmount, new ValidationContext(this, null, null) { MemberName = "HeatingAmount" }, results);

            return results;
        }
    }
}