
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotTracktorBrain
{
    public class Output
    {
        const byte DEFAULT_POTENTIAL = 10;
        public uint[] id;
        public byte potential;

        public Output(uint[] Id)
        {
            id = Id;
            potential = DEFAULT_POTENTIAL;
        }
    }

    public class Input
    {
        public uint[] id;
        public byte value = 0;
        public bool active = false;

        public Input(uint[] Id)
        {
            id = Id;
        }
    }
    public class Neuron
    {
        public uint[] id;
        public byte potential;
        public List<Output> Axon;
        public List<Input> Dendrits;

        public Neuron(uint[] id, byte potential)
        {
            this.id = id;
            this.potential = potential;
        }

        public void ProcessInputs()
        {
            uint adoptedPotential = potential;
            foreach (var input in Dendrits)
            {
                if (input.value > 0)
                {
                    adoptedPotential += input.value;
                    input.active = true;
                }
                else
                {
                    input.active = false;
                }
            }

            if (adoptedPotential > 255)
            {
                adoptedPotential = 255;
            }

            potential = (byte)adoptedPotential;
        }

        public bool Reaction()
        {
            bool discharge = false;
            Random random = new Random();

            double probability = (double)(Math.Abs((int)potential - 127)) / 128.0;
            if(probability >= 1.0)
            {
                probability = 0.99;
            }
            else if(probability <= 0.0)
            {
                probability = 0.01;
            }

            discharge = random.NextDouble() < probability;

            return discharge;
        }

        
    }
}
