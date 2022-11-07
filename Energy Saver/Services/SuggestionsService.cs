using Energy_Saver.Model;

namespace Energy_Saver.Services
{
    public class SuggestionsService : ISuggestionsService
    {

        public Taxes GetLatestTaxComparison()
        {
            List<List<Taxes>> list = Serialization.ReadFromFile();

            Taxes comparison = new Taxes();

            comparison.GasAmount = Math.Round(list.ElementAt(0).ElementAt(0).GasAmount * 100 / list.ElementAt(0).ElementAt(1).GasAmount - 100, 1);
            comparison.ElectricityAmount = Math.Round(list.ElementAt(0).ElementAt(0).ElectricityAmount * 100 / list.ElementAt(0).ElementAt(1).ElectricityAmount - 100, 1);
            comparison.WaterAmount = Math.Round(list.ElementAt(0).ElementAt(0).WaterAmount * 100 / list.ElementAt(0).ElementAt(1).WaterAmount - 100, 1);
            comparison.HeatingAmount = Math.Round(list.ElementAt(0).ElementAt(0).HeatingAmount * 100 / list.ElementAt(0).ElementAt(1).HeatingAmount - 100, 1);

            return comparison;
        }
    }
}
