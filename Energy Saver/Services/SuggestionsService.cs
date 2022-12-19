using Energy_Saver.Model;
using static Energy_Saver.Services.ISuggestionsService;

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
                if(CheckForZeros(list))
                {
                    comparison.Add(Math.Round(list.First().GasAmount * 100 / list.ElementAt(1).GasAmount - 100));
                    comparison.Add(Math.Round(list.First().ElectricityAmount * 100 / list.ElementAt(1).ElectricityAmount - 100));
                    comparison.Add(Math.Round(list.First().WaterAmount * 100 / list.ElementAt(1).WaterAmount - 100));
                    comparison.Add(Math.Round(list.First().HeatingAmount * 100 / list.ElementAt(1).HeatingAmount - 100));
                }
            }

            return comparison;
        }

        public bool CheckForZeros(List<Taxes> list)
        {
            if (list.ElementAt(1).GasAmount != 0 && list.ElementAt(1).ElectricityAmount != 0 && list.ElementAt(1).WaterAmount != 0 && list.ElementAt(1).HeatingAmount != 0)
            {
                return true;
            }
            return false;
        }

        public List<TaxesWithSum> PercetangeAboveOrBelowAverage(List<List<Taxes>> taxes)
        {
            decimal averageAllTime = taxes.SelectMany(taxesList => taxesList)
                .Select(taxes => taxes.HeatingAmount + taxes.ElectricityAmount + taxes.WaterAmount + taxes.GasAmount)
                .Average();

            List<(Months, int)> comb = taxes.SelectMany(taxesList => taxesList).Select(taxes => (taxes.Month, taxes.Year)).ToList();

            List<TaxesWithSum> result = new List<TaxesWithSum>();

            foreach((var month, var year) in comb)
            {
                var monthSum = taxes.SelectMany(taxesList => 
                                                    taxesList.Where(x => x.Month == month && x.Year == year)
                                                             .Select(taxes => taxes.HeatingAmount + taxes.ElectricityAmount + taxes.WaterAmount + taxes.GasAmount)
                                               ).First();

                var calculation = averageAllTime == 0 ? 0 : Math.Round(((monthSum - averageAllTime) / Math.Abs(averageAllTime)) * 100, 2);

                result.Add(
                    new TaxesWithSum 
                    { 
                        Year = year, 
                        Month = month, 
                        Percentage = calculation
                    }
                );
            }

            return result;
        }

    }
}
