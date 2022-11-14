using Energy_Saver.Model;

namespace Energy_Saver.Services
{
    public interface ISuggestionsService
    {
        public List<decimal> GetLatestTaxComparison();
    }
}
