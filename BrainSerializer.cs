using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace RobotTracktorBrain
{
    public class BrainSerializer
    {
        public static void SerializeBrainMap(string baseDirectoryPath)
        {
            var brain = Brain.Instance;
            // Ensure the base directory exists
            Directory.CreateDirectory(baseDirectoryPath);

            for (uint i = 0; i < Brain.BRAIN_WIDTH; i++)
            {
                // Create a directory for each row
                string rowDirectoryPath = Path.Combine(baseDirectoryPath, $"row_{i}");
                Directory.CreateDirectory(rowDirectoryPath);

                for (uint j = 0; j < Brain.BRAIN_HEIGHT; j++)
                {
                    SerializeColumn(i, j, brain.brainMap, rowDirectoryPath);
                }
            }
        }

        private static void SerializeColumn(uint x, uint y, Neuron[,,] brainMap, string rowDirectoryPath)
        {
            var column = new List<Neuron>();

            for (uint depth = 0; depth < Brain.BRAIN_DEPTH; depth++)
            {
                // Assuming the neuron at [x, y, depth] is not null
                column.Add(brainMap[x, y, depth]);
            }

            string filePath = Path.Combine(rowDirectoryPath, $"column_{y}.json");
            string json = JsonConvert.SerializeObject(column, Formatting.Indented);

            File.WriteAllText(filePath, json);
        }
    }
}
