using Energy_Saver.Model;

namespace Energy_Saver.Services
{
    public class SuggestionsService : ISuggestionsService
    {
        public decimal GetLatestGasComparison()
        {
            List<List<Taxes>> list = Serialization.ReadFromFile();

            decimal newestGas = list.ElementAt(0).ElementAt(0).GasAmount;
            decimal secondNewestGas = list.ElementAt(0).ElementAt(1).GasAmount;

            return Math.Round(newestGas * 100 / secondNewestGas - 100, 1);
        }

        public decimal GetLatestElectricityComparison()
        {
            List<List<Taxes>> list = Serialization.ReadFromFile();

            decimal newestElectricity = list.ElementAt(0).ElementAt(0).ElectricityAmount;
            decimal secondNewestElectricity = list.ElementAt(0).ElementAt(1).ElectricityAmount;

            return Math.Round(newestElectricity * 100 / secondNewestElectricity - 100, 1);
        }

        public decimal GetLatestWaterComparison()
        {
            List<List<Taxes>> list = Serialization.ReadFromFile();

            decimal newestWater = list.ElementAt(0).ElementAt(0).WaterAmount;
            decimal secondNewestWater = list.ElementAt(0).ElementAt(1).WaterAmount;

            return Math.Round(newestWater * 100 / secondNewestWater - 100, 1);
        }

        public decimal GetLatestHeatingComparison()
        {
            List<List<Taxes>> list = Serialization.ReadFromFile();

            decimal newestHeating = list.ElementAt(0).ElementAt(0).HeatingAmount;
            decimal secondNewestHeating = list.ElementAt(0).ElementAt(1).HeatingAmount;

            return Math.Round(newestHeating * 100 / secondNewestHeating - 100, 1);
        }

    }
}
