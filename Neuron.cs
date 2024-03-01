
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotTracktorBrain
{
    public class Output
    {
        public const byte DEFAULT_POTENTIAL = 10;
        public const byte POTENTIAL_ACTIVITY_INCREMENT = 3;
        public const byte POTENTIAL_INACTIVITY_DECREMENT = 1;
        public const byte MAX_POTENTIAL = 100;
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
        private static Random random = new Random();

        public bool dischargeFlag = false;

        const byte TRANSMITTED_POTENTIAL = 1;
        public const byte MAX_NEW_BOND_POTENTIAL = 7;

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
                    input.value = 0;
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

        public void CalculateReaction()
        {
            double probability = (double)(Math.Abs((int)potential - 127)) / 128.0;
            if(probability >= 1.0)
            {
                probability = 0.99;
            }
            else if(probability <= 0.0)
            {
                probability = 0.01;
            }

            dischargeFlag = random.NextDouble() < probability;
        }

        public void Discharge()
        {
            // Sorting Axon in descending order based on potential
            Axon.Sort((x, y) => y.potential.CompareTo(x.potential));

            // in the queue play probability for every single sinaps
            foreach(var output in Axon) 
            {
                if(potential < TRANSMITTED_POTENTIAL)
                {
                    potential = 0; break;
                }

                double probability = (double)output.potential / Output.MAX_POTENTIAL;
                bool discharge = random.NextDouble() < probability;

                if(!discharge) 
                {
                    if(potential > Output.POTENTIAL_INACTIVITY_DECREMENT)
                    {
                        potential -= Output.POTENTIAL_INACTIVITY_DECREMENT;
                    }
                    else
                    {
                        potential = 0;
                    }
                }
                else // discharge via this sinaps
                {
                    var pairedNeuron = Brain.Instance.brainMap[output.id[0], output.id[1], output.id[2]];
                    var pairedInput = pairedNeuron.Dendrits.FirstOrDefault(input => input.id.SequenceEqual(output.id));
                    pairedInput.value += TRANSMITTED_POTENTIAL;
                    potential -= TRANSMITTED_POTENTIAL;
                }
            }

        }

        public void UtilizeInactiveOutputs()
        {
            foreach(var output in Axon)
            {
                if(output.potential == 0)
                {
                    var pairedNeuronId = output.id;
                    var pairedNeuron = Brain.Instance.brainMap[pairedNeuronId[0], pairedNeuronId[1], pairedNeuronId[2]];
                    // remove input from paired neuron
                    pairedNeuron.Dendrits.RemoveAll(input => input.id.SequenceEqual(id));
                }
                Axon.Remove(output);
            }
            //Axon.RemoveAll(output => output.potential == 0);
        }

        public void CreateNewBonds()
        {
            if(dischargeFlag)
            {
                while(potential > 0)
                {
                    uint[] newBondId;
                    do
                    {
                        newBondId = GenerateRandomId();
                    } while (Axon.Any(output => output.id.SequenceEqual(newBondId)));

                    byte newBondPotential = (byte)random.Next(1, MAX_NEW_BOND_POTENTIAL + 1);
                    newBondPotential = Math.Min(newBondPotential, potential);
                    potential -= newBondPotential;

                    var newBondNeuron = Brain.Instance.brainMap[newBondId[0], newBondId[1], newBondId[2]];
                    var newInput = new Input(id);
                    newInput.value = 0;
                    newInput.active = false;
                    newBondNeuron.Dendrits.Add(newInput);

                    var newOutput = new Output(id);
                    newOutput.potential = newBondPotential;
                    Axon.Add(newOutput);
                }
            }
        }

        private static uint[] GenerateRandomId()
        {
            return new uint[3] 
            {
                (uint)random.Next(0, (int)Brain.BRAIN_WIDTH),
                (uint)random.Next(0, (int)Brain.BRAIN_HEIGHT),
                (uint)random.Next(1, (int)Brain.BRAIN_DEPTH)
            };
        }

    }
}
