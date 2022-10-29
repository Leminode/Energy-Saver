using Energy_Saver.Model;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Energy_Saver.Model
{
    public static class Serialization
    {
        private static string path = "Resources/Taxes.json";

        private static JsonSerializerSettings CamelCaseJsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public static List<List<Taxes>> ReadFromFile()
        {
            List<List<Taxes>> taxes;

            List<Taxes>? temp = ReadText();
        
            taxes = OrderList(SortDirection.Descending, temp, tax => tax.Month).GroupBy(t => t.Year).Select(year => year.ToList()).ToList();
            taxes = OrderList(SortDirection.Descending, taxes, taxes => taxes[0]);

            return taxes;
        }

        public static void WriteText(List<Taxes> taxes)
        {
            string serializedString = JsonConvert.SerializeObject(taxes, Formatting.Indented, CamelCaseJsonSerializerSettings);

            File.WriteAllText(Path.GetFullPath(path), serializedString);
        }

        public static void WriteEntryToFile(Taxes taxes)
        {
            List<Taxes>? newList = ReadText();

            int duplicateIndex = newList.FindIndex(tax => tax.Year.Equals(taxes.Year) && tax.Month.Equals(taxes.Month));

            //If duplicate was found
            if (duplicateIndex != -1)
                newList[duplicateIndex] = taxes;
            else
                newList.Add(taxes);

            WriteText(newList);
        }

        private static List<Taxes> ReadText()
        {
            string json = File.ReadAllText(Path.GetFullPath(path));

            List<Taxes>? temp = JsonConvert.DeserializeObject<List<Taxes>>(json, CamelCaseJsonSerializerSettings);

            return temp;
        }

        public static List<T> OrderList<T, U>(SortDirection sortDirection, List<T> data, Func<T, U> sortBy)
        {
            List<T> orderedList = new List<T>();

            switch (sortDirection)
            {
                case SortDirection.Ascending:
                    orderedList = (from n in data orderby sortBy ascending select n).ToList();
                    break;
                case SortDirection.Descending:
                    orderedList = (from n in data orderby sortBy descending select n).ToList();
                    break;
                default:
                    return data;
            }
            return orderedList;
        }

        public enum SortDirection
        {
            Ascending,
            Descending
        }
    }
}
