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
        
            taxes = temp.GroupBy(t => t.Year).Select(year => year.ToList()).ToList();
            taxes = taxes.Select(tax => OrderList<Taxes>(data: tax, sortExpression: "Month", sortDirection: "Descending")).OrderByDescending(t => t[0]).ToList();

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

        public static List<T> OrderList<T>(string sortDirection, string sortExpression, List<T> data)
        {
            List<T> orderedList = new List<T>();

            switch (sortDirection)
            {
                case "Ascending":
                    orderedList = (from n in data orderby GetDynamicSortProperty(n, sortExpression) ascending select n).ToList();
                    break;
                case "Descending":
                    orderedList = (from n in data orderby GetDynamicSortProperty(n, sortExpression) descending select n).ToList();
                    break;
                default:
                    break;
            }
            return orderedList;
        }

        private static object GetDynamicSortProperty(object item, string propName)
        {
            return item.GetType().GetProperty(propName).GetValue(item, null);
        }
    }
}
