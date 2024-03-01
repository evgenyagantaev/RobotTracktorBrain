
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotTracktorBrain
{
    
    public class Brain
    {
        public Neuron[,,] brain;

        public Brain(uint width, uint height, uint depth)
        {
            brain = new Neuron[width, height, depth];

            Random rand = new Random();

            int randomNumber = rand.Next(0, 256);

            for (uint i = 0; i < width; i++) 
            {
                for(uint j = 0; j < height; j++)
                {
                    for (uint k = 0; k < depth; k++)
                    {
                        brain[i, j, k] = new Neuron(new uint[] {i, j, k}, (byte)rand.Next(0, 256));

                    }
                }
            }
        }
    }
}
