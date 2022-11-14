using Energy_Saver.Model;

namespace Energy_Saver.Services
{
    public interface ISuggestionsService
    {
        public decimal GetLatestGasComparison(List<List<Taxes>> list);

        public decimal GetLatestWaterComparison(List<List<Taxes>> list);

        public decimal GetLatestElectricityComparison(List<List<Taxes>> list);

        public decimal GetLatestHeatingComparison(List<List<Taxes>> list);
    }
}
