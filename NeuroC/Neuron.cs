using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuroC
{
    class Neuron
    {
        public double[] contactsValue;
        public double output;
        public double delta;

        public Neuron(int cuantityContacts, double minRange, double maxRange,Random rand)
        {
            contactsValue = new double[cuantityContacts];
            output = 0;

            for (int i = 0; i < cuantityContacts; i++)
            {
                contactsValue[i] = (double)(rand.Next(-30, 30)) / 100;
                 if (contactsValue[i] > 0.3 || contactsValue[i] < -0.3)
                     i--;
            }
        }

        public double ForwardFlow(double[] outputPreviousLayer)
        {
            output = 0;
            for (int i = 0; i < outputPreviousLayer.Length; i++)
            {
                output += outputPreviousLayer[i] * contactsValue[i];
            }

            output = 1/(1 + Math.Exp(-output));
            return output;
        }
    }
}
