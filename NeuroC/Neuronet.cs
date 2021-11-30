using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuroC
{
    class Neuronet
    {
        public Layer[] layers;
        double eta;
        Random rand = new Random();

        public Neuronet(int quantityLayers, int quantityInputs, int quantityOutputs, int quantityNeurons, double minRange, double maxRange, double eta)
        {
            this.eta = eta;
            layers = new Layer[quantityLayers];

            layers[0] = new Layer(quantityNeurons,quantityInputs,minRange,maxRange,rand);
            for (int i = 1; i < quantityLayers - 1; i++)
            {
                layers[i] = new Layer(quantityNeurons, layers[i - 1].neurons.Length, minRange, maxRange,rand);
            }
            layers[layers.Length - 1] = new Layer(quantityOutputs, layers[layers.Length - 2].neurons.Length, minRange, maxRange,rand);
        }


        public double[] ForwardFlow(double[] inputs)
        {
            layers[0].ForwardFlow(inputs);
            for (int i = 1; i < layers.Length; i++)
            {
                layers[i].ForwardFlow(layers[i - 1].outputsLayer);
            }
            double[] outputsNet = (double[])layers[layers.Length - 1].outputsLayer.Clone();
            return layers[layers.Length - 1].outputsLayer;
        }


        public double TraningNet(double[] inputs, double[] target)
        {
            ForwardFlow(inputs);
            //вычисляем ошибки нейронов
            //последнего слоя
            if (layers.Length > 1)
            {
                for (int j = 0; j < layers[layers.Length - 1].neurons.Length; j++)
                {
                    layers[layers.Length - 1].neurons[j].delta =
                        layers[layers.Length - 1].neurons[j].output *
                        (1 - layers[layers.Length - 1].neurons[j].output) *
                        (target[j] - layers[layers.Length - 1].neurons[j].output);

                    for (int k = 0; k < layers[layers.Length - 2].neurons.Length; k++) //корректируем веса последнего слоя если они скрытые 
                    {
                        layers[layers.Length - 1].neurons[j].contactsValue[k] += eta * layers[layers.Length - 1].neurons[j].delta * layers[layers.Length - 2].neurons[k].output;
                    }
                }

                //вычисляем ошибки нейронов слоёв, следующих от выходного слоя
                double sumDeltaW;
                for (int i = layers.Length - 2; i >= 0; i--)
                {
                    for (int j = 0; j < layers[i].neurons.Length; j++)
                    {
                        sumDeltaW = 0;
                        for (int k = 0; k < layers[i+1].neurons.Length; k++)
                        {
                            sumDeltaW += layers[i+1].neurons[k].delta*
                                layers[i+1].neurons[k].contactsValue[j];
                        }
                        layers[i].neurons[j].delta =
                            layers[i].neurons[j].output *
                            (1 - layers[i].neurons[j].output) * sumDeltaW;

                        // корректируем веса
                        for (int k = 0; k < layers[i].neurons[j].contactsValue.Length; k++)
                        {
                            if (i != 0) // скрытые веса
                            {
                                layers[i].neurons[j].contactsValue[k] += eta * layers[i].neurons[j].delta * layers[i - 1].neurons[k].output;
                            }
                            else        //первые веса
                            {
                                layers[i].neurons[j].contactsValue[k] += eta * layers[i].neurons[j].delta * inputs[k];
                            }
                        }
                    }
                }
            }
            else //если слой один
            {
                for (int j = 0; j < layers[layers.Length - 1].neurons.Length; j++)
                {
                    layers[layers.Length - 1].neurons[j].delta =
                        layers[layers.Length - 1].neurons[j].output *
                        (1 - layers[layers.Length - 1].neurons[j].output) *
                        (target[j] - layers[layers.Length - 1].neurons[j].output);

                    for (int k = 0; k < layers[layers.Length - 2].neurons.Length; k++) //корректируем веса последнего слоя если они входные (сеть однослойня)
                    {
                        layers[layers.Length - 1].neurons[j].contactsValue[k] += eta * layers[layers.Length - 1].neurons[j].delta * inputs[k];
                    }
                }
            }
            double sqrError = 0;
            for (int i = 0; i<target.Length;i++)
            {
                sqrError += Math.Pow((layers[layers.Length-1].outputsLayer[i]-target[i]),2)/2;
            }
            return sqrError;


            //вычисляем ошибки нейронов
            //скрытых слоев


            //double sumDeltaW;
            //if (layers.Length > 1)
            //{
            //    for (int i = layers.Length - 2; i >= 0; i--)
            //    {
            //        for (int j = 0; j < layers[i].neurons.Length; j++)
            //        {
            //            sumDeltaW = 0;
            //            for (int k = 0; k < layers[i+1].neurons.Length; k++)
            //            {
            //                sumDeltaW += layers[i+1].neurons[k].delta*
            //                    layers[i+1].neurons[k].contactsValue[j];
            //            }
            //            layers[i].neurons[j].delta =
            //                layers[i].neurons[j].output *
            //                (1 - layers[i].neurons[j].output) * sumDeltaW;
            //            // корректируем веса

            //        }
            //    }
            //}

            ////корректируем веса
            //for (int i = layers.Length - 2; i >= 0; i--)
            //{
            //    for (int j = 0; j < layers[i].neurons.Length; j++)
            //    {
            //        for (int k = 0; k < layers[i].neurons[j].contactsValue.Length; k++)
            //        {
            //            if (i != 0)
            //            {
            //                layers[i].neurons[j].contactsValue[k] += eta * layers[i].neurons[j].delta * layers[i - 1].neurons[k].output;
            //            }
            //            else
            //            {
            //                layers[i].neurons[j].contactsValue[k] += eta * layers[i].neurons[j].delta * inputs[k];
            //            }
            //        }
            //    }
            //}
        }
    }
}
