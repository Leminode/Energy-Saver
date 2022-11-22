using Energy_Saver.Model;

namespace Energy_Saver.Services
{
    public class SuggestionsService : ISuggestionsService
    {

        public List<decimal> GetLatestTaxComparison(List<List<Taxes>> taxes)
        {
            List<Taxes> list = taxes.SelectMany(x => x).ToList();

            List<decimal> comparison = new List<decimal>();

            if(list.Count > 1)
            {
                if(list.ElementAt(1).GasAmount != 0 && list.ElementAt(1).ElectricityAmount != 0 && list.ElementAt(1).WaterAmount != 0 && list.ElementAt(1).HeatingAmount != 0)
                {
                    comparison.Add(Math.Round(list.First().GasAmount * 100 / list.ElementAt(1).GasAmount - 100));
                    comparison.Add(Math.Round(list.First().ElectricityAmount * 100 / list.ElementAt(1).ElectricityAmount - 100));
                    comparison.Add(Math.Round(list.First().WaterAmount * 100 / list.ElementAt(1).WaterAmount - 100));
                    comparison.Add(Math.Round(list.First().HeatingAmount * 100 / list.ElementAt(1).HeatingAmount - 100));
                }
            }

            return comparison;
        }
    }
}
