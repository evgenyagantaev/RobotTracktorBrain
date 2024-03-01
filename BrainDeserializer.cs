using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace RobotTracktorBrain
{
    public class BrainDeserializer
    {
        public static Neuron[,,] DeserializeBrainMap(string directoryPath, uint width, uint height, uint depth)
        {
            Neuron[,,] brainMap = new Neuron[width, height, depth];

            for (uint d = 0; d < depth; d++)
            {
                string filePath = Path.Combine(directoryPath, $"brainMap_layer_{d}.json");
                if (File.Exists(filePath))
                {
                    DeserializeLayer(brainMap, d, filePath);
                }
            }

            return brainMap;
        }

        private static void DeserializeLayer(Neuron[,,] brainMap, uint depth, string filePath)
        {
            string json = File.ReadAllText(filePath);
            List<Neuron> layer = JsonConvert.DeserializeObject<List<Neuron>>(json);

            if (layer != null)
            {
                for (int i = 0; i < layer.Count; i++)
                {
                    uint x = layer[i].id[0];
                    uint y = layer[i].id[1];
                    uint z = depth; // Assuming depth is consistent with the file naming
                    brainMap[x, y, z] = layer[i];
                }
            }
        }
    }
}
