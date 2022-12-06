using Energy_Saver.Model;

namespace Energy_Saver.Services
{
    public interface ISuggestionsService
    {
        public List<decimal> GetLatestTaxComparison(List<List<Taxes>> taxes);
        public decimal PercetangeAboveOrBelowAverage(List<List<Taxes>> taxes, Months month, int year);
    }
}
