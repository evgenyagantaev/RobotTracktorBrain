
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotTracktorBrain
{
    
    public class Brain
    {
        public const uint BRAIN_WIDTH = 136;
        public const uint BRAIN_HEIGHT = 136;
        public const uint BRAIN_DEPTH = 10;
        public const uint INPUT_ZONE_WIDTH = 120;
        public const uint INPUT_ZONE_HEIGHT = 120;
        public const uint OUTPUT_ZONE_WIDTH = 120;
        public const uint OUTPUT_ZONE_HEIGHT = 120;

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

        public void feedforwardProcess()
        {
            for (uint d = 0; d < BRAIN_DEPTH - 2; d--)
            {
                for (int x = 0; x < BRAIN_WIDTH; x++)
                {
                    for (int y = 0; y < BRAIN_HEIGHT; y++)
                    {
                        var neuron = brainMap[x, y, d];
                        neuron.ProcessInputs();
                        neuron.CalculateReaction(); // calculate discharge event
                        if (neuron.dischargeFlag)
                        {
                            neuron.Discharge();
                        }
                        neuron.UtilizeInactiveOutputs();
                        neuron.CreateNewBonds();
                    }
                }
            }
        }

        public void feedbackProcess(byte stimulationValue)
        {
            for(int d = (int)(BRAIN_DEPTH-1); d >= 0; d--)
            {
                for (int x = 0; x < BRAIN_WIDTH; x++)
                {
                    for (int y = 0; y < BRAIN_HEIGHT; y++)
                    {
                        brainMap[x, y, d].StimulateBonds(stimulationValue);
                    }
                }
            }
        }
    }
}
