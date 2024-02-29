
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
    }
}
