using Energy_Saver.Model;

namespace Energy_Saver.Services
{
    public class TableService : ITableService
    {
        public List<List<Taxes>> GetTableContents()
        {

            List<List<Taxes>> temp = Serialization.ReadFromFile();

            return temp;
        }

        public void DeleteEntry(int monthIndex, int yearIndex)
        {
            List<List<Taxes>> taxes;

            taxes = Serialization.ReadFromFile();
            taxes[yearIndex].RemoveAt(monthIndex);

            Serialization.WriteText(taxes.SelectMany(list => list).Distinct().ToList());
        }

        public void AddEntry(Taxes taxes)
        {
            Serialization.WriteEntryToFile(taxes);
        }
    }
}
