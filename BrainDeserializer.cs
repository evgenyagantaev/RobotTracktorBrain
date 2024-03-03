using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace RobotTracktorBrain
{
    public class BrainDeserializer
    {
        public static Neuron[,,] DeserializeBrainMap(string baseDirectoryPath, uint width, uint height, uint depth)
        {
            Neuron[,,] brainMap = new Neuron[width, height, depth];

            for (uint i = 0; i < width; i++)
            {
                string rowDirectoryPath = Path.Combine(baseDirectoryPath, $"row_{i}");
                if (Directory.Exists(rowDirectoryPath))
                {
                    for (uint j = 0; j < height; j++)
                    {
                        string filePath = Path.Combine(rowDirectoryPath, $"column_{j}.json");
                        if (File.Exists(filePath))
                        {
                            DeserializeColumn(brainMap, i, j, filePath);
                        }
                    }
                }
            }

            return brainMap;
        }

        private static void DeserializeColumn(Neuron[,,] brainMap, uint x, uint y, string filePath)
        {
            string json = File.ReadAllText(filePath);
            List<Neuron> column = JsonConvert.DeserializeObject<List<Neuron>>(json);

            if (column != null)
            {
                for (uint depth = 0; depth < column.Count; depth++)
                {
                    // Assuming the depth index matches the order in the list
                    if (depth < column.Count)
                    {
                        brainMap[x, y, depth] = column[(int)depth];
                    }
                }
            }
        }
    }
}
