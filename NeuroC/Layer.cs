using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuroC
{
    class Layer
    {
        public Neuron[] neurons;
        public double[] outputsLayer;

        public Layer(int quantityNeurons, int quantityNeuronsOnPreviousLayer, double minRange, double maxRange,Random rand)
        {
            neurons = new Neuron[quantityNeurons];
            outputsLayer = new double[quantityNeurons];

            for (int i = 0; i < quantityNeurons; i++)
            {
                neurons[i] = new Neuron(quantityNeuronsOnPreviousLayer, minRange, maxRange,rand);
            }
        }

        public void ForwardFlow(double[] outputPreviousLayer)
        {
            for (int i = 0; i < neurons.Length; i++)
            {
                outputsLayer[i] = neurons[i].ForwardFlow(outputPreviousLayer);
            }
        }
    }
}
