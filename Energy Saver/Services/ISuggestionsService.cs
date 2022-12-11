using Energy_Saver.Model;

namespace Energy_Saver.Services
{
    public interface ISuggestionsService
    {
        public List<decimal> GetLatestTaxComparison(List<List<Taxes>> taxes);
        public List<TaxesWithSum> PercetangeAboveOrBelowAverage(List<List<Taxes>> taxes);

        public struct TaxesWithSum
        {
            public int Year { get; set; }
            public Months Month { get; set; }
            public decimal Percentage { get; set; }
            public string Style { get; set; }
            public string Icon { get; set; }
        }
    }
}
