using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#nullable enable

public class Utils
{
    public static bool WriteJsonToFile(string jsonPath, JObject jsonObject) {
        try {
            File.AppendAllText(jsonPath, jsonObject.ToString());
            return true;
        } catch (Exception)
        {
            return false;
        }
        
    }
    public static JObject ConvertTextToJson(string jsonString) {
        try {
         JObject? json = JObject.Parse(jsonString);
            return json;
        } catch (Exception e) {
            Console.WriteLine($"Error loading JSON from {jsonString}: {e.Message}");
            return new JObject();
        }
    }

    public static JObject OpenJsonFile(string jsonPath) {
        try {
            JObject? json = JObject.Parse(File.ReadAllText(jsonPath));
            return json;
        } catch (Exception e) {
            Console.WriteLine($"Error loading JSON from {jsonPath}: {e.Message}");
            return new JObject();
        }
    }


   public static JObject MergeJsonWithAverages(string filePath, JObject newJson)
    {
        JObject oldJson;

        // Read existing JSON from file
        if (File.Exists(filePath))
        {
            var fileContent = File.ReadAllText(filePath);
            oldJson = JObject.Parse(fileContent);
        }
        else
        {
            oldJson = new JObject();
        }

        foreach (var property in newJson.Properties())
        {
            string key = property.Name;
            double newValue = property.Value.ToObject<double>();

            if (oldJson.ContainsKey(key))
            {
                #pragma warning disable
                double? oldValue = oldJson[key].ToObject<double>();
                double? average = (oldValue + newValue) / 2.0;
                oldJson[key] = average;
            }
            else
            {
                oldJson[key] = newValue;
            }
        }

        // Save updated JSON back to file
        File.WriteAllText(filePath, oldJson.ToString(Formatting.Indented));
        return oldJson;
    }


}
