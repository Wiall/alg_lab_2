using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace alg_lab_2
{
    public class MapColoringResultSaver
    {
        public static void SaveResult(Dictionary<int, int> coloring, string filePath)
        {
            try
            {
                var jsonResult = JsonConvert.SerializeObject(coloring, Formatting.Indented);
                File.WriteAllText(filePath, jsonResult);
                Console.WriteLine($"Solution saved to {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to file: {ex.Message}");
            }
        }
    }
}