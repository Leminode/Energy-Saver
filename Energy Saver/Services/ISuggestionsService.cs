using Energy_Saver.Model;

namespace Energy_Saver.Services
{
    public interface ISuggestionsService
    {
        public decimal GetLatestGasComparison();

        public decimal GetLatestWaterComparison();

        public decimal GetLatestElectricityComparison();

        public decimal GetLatestHeatingComparison();
    }
}
