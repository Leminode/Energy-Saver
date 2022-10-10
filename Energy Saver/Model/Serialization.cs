using Energy_Saver.Model;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;

public static class Serialization
{
    private static string path = "Resources/Taxes.json";

    private static JsonSerializerSettings serializerSettings = new JsonSerializerSettings
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver()
    };

    public static List<List<Taxes>> ReadFromFile(this PageModel pageModel)
    {
        List<List<Taxes>> taxes;

        List<Taxes>? temp = ReadText();
        
        taxes = temp.GroupBy(t => t.Year).Select(year => year.ToList()).ToList();
        taxes = taxes.Select(taxes => taxes.OrderByDescending(i => i.Month).ToList()).OrderByDescending(t => t[0]).ToList();

        return taxes;
    }

    public static void WriteToFile(this PageModel pageModel, Taxes taxes)
    {
        List<Taxes>? temp = ReadText();

        temp.Add(taxes);

        string serializedString = JsonConvert.SerializeObject(temp, Formatting.Indented, serializerSettings);

        File.WriteAllText(Path.GetFullPath(path), serializedString);
    }

    private static List<Taxes> ReadText()
    {
        string json = File.ReadAllText(Path.GetFullPath(path));

        List<Taxes>? temp = JsonConvert.DeserializeObject<List<Taxes>>(json, serializerSettings);

        return temp;
    }
}