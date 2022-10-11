﻿using Energy_Saver.Model;
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

    public static void WriteText(this PageModel pageModel, List<Taxes> taxes)
    {
        string serializedString = JsonConvert.SerializeObject(taxes, Formatting.Indented, serializerSettings);

        File.WriteAllText(Path.GetFullPath(path), serializedString);
    }

    public static void WriteEntryToFile(this PageModel pageModel, Taxes taxes)
    {
        List<Taxes>? newList = ReadText();

        int duplicateIndex = newList.FindIndex(tax => tax.Year.Equals(taxes.Year) && tax.Month.Equals(taxes.Month));

        //If duplicate was found
        if (duplicateIndex != -1)
            newList[duplicateIndex] = taxes;
        else
            newList.Add(taxes);

        string serializedString = JsonConvert.SerializeObject(newList, Formatting.Indented, serializerSettings);

        File.WriteAllText(Path.GetFullPath(path), serializedString);
    }

    private static List<Taxes> ReadText()
    {
        string json = File.ReadAllText(Path.GetFullPath(path));

        List<Taxes>? temp = JsonConvert.DeserializeObject<List<Taxes>>(json, serializerSettings);

        return temp;
    }
}