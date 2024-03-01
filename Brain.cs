
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotTracktorBrain
{
    
    public class Brain
    {
        public const uint BRAIN_WIDTH = 3000;
        public const uint BRAIN_HEIGHT = 3000;
        public const uint BRAIN_DEPTH = 100;
        public const uint INPUT_ZONE_WIDTH = 2000;
        public const uint INPUT_ZONE_HEIGHT = 2000;

        private static Brain instance = null;

        public Neuron[,,] brainMap;

        private Brain(uint width, uint height, uint depth)
        {
            brainMap = new Neuron[width, height, depth];

            Random rand = new Random();

            int randomNumber = rand.Next(0, 256);

            for (uint i = 0; i < width; i++) 
            {
                for(uint j = 0; j < height; j++)
                {
                    for (uint k = 0; k < depth; k++)
                    {
                        brainMap[i, j, k] = new Neuron(new uint[] {i, j, k}, (byte)rand.Next(0, 256));

                    }
                }
            }
        }

        public static Brain Instance
        {
            get
            {
                // If the instance doesn't exist, create it
                if (instance == null)
                {
                    instance = new Brain(BRAIN_WIDTH, BRAIN_HEIGHT, BRAIN_DEPTH);
                }
                return instance;
            }
        }
    }
}
