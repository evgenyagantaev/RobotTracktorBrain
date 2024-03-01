using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace RobotTracktorBrain
{
    public class BrainSerializer
    {
        public static void SerializeBrainMap(string directoryPath)
        {
            var brain = Brain.Instance;
            for (uint depth = 0; depth < Brain.BRAIN_DEPTH; depth++)
            {
                SerializeLayer(brain.brainMap, depth, directoryPath);
            }
        }

        private static void SerializeLayer(Neuron[,,] brainMap, uint depth, string directoryPath)
        {
            var layer = new List<Neuron>();

            for (uint i = 0; i < Brain.BRAIN_WIDTH; i++)
            {
                for (uint j = 0; j < Brain.BRAIN_HEIGHT; j++)
                {
                    // Assuming that the neuron at [i, j, depth] is not null
                    layer.Add(brainMap[i, j, depth]);
                }
            }

            string filePath = Path.Combine(directoryPath, $"brainMap_layer_{depth}.json");
            string json = JsonConvert.SerializeObject(layer, Formatting.Indented);

            File.WriteAllText(filePath, json);
        }
    }
}
