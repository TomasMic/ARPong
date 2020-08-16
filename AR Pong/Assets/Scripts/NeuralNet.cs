using System.Collections;
using System.Collections.Generic;
using System;


public class NeuralNet
{
    private int[] layers; //store number of neurons in layers
    private float[][] neurons;
    public float[][][] weights;
    public float mutationChance = 10f;
    public float fitness = 0; 

    public NeuralNet(int[] layers)
    {
        this.layers = new int[layers.Length];
        for(int i = 0; i < layers.Length; i++) //create layers
        {
            this.layers[i] = layers[i];
        } 
        InitializeNeurons();
        InitializeWeights();
    }

    

    public NeuralNet (NeuralNet copyNetwork) //deep copy of neural net
    {
        this.layers = new int[copyNetwork.layers.Length];
        for (int i = 0; i < copyNetwork.layers.Length; i++) //create layers
        {
            this.layers[i] = copyNetwork.layers[i];
        }

        InitializeNeurons();
        InitializeWeights();
        CopyWeights(copyNetwork.weights);
    }

    private void InitializeNeurons()
    {
        List<float[]> neuronsList = new List<float[]>();

        for (int i = 0; i < layers.Length; i++) //in every layer
        {
            neuronsList.Add(new float[layers[i]]); //add neurons baed on ho many is set
        }
        neurons = neuronsList.ToArray();
    }
    private void InitializeWeights()
    {
        List<float[][]> weightsList = new List<float[][]>();
        for (int i = 1; i < layers.Length; i++) //in every layer
        {
            List<float[]> layerWeights = new List<float[]>(); //weights in layer

            int prevNeurons = layers[i - 1]; //neurons in previous layer

            for (int j = 0; j < neurons[i].Length; j++)
            {
                float[] neuronWeights = new float[prevNeurons];

                for (int k = 0; k < prevNeurons; k++) //only to give weights some values
                {
                    neuronWeights[k] = UnityEngine.Random.Range(-0.5f, 0.5f);
                }
                layerWeights.Add(neuronWeights);
            }
            weightsList.Add(layerWeights.ToArray());
        }
        weights = weightsList.ToArray();
    }

    private void CopyWeights(float[][][] weightsToCopy)
    {
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++) //checking trough all weights
                {
                    weights[i][j][k] = weightsToCopy[i][j][k];
                }
            }
        }
    }

    public float[] FeedForward(float[] inputs)
    {
        //Add inputs to the neuron matrix
        for (int i = 0; i < inputs.Length; i++)
        {
            neurons[0][i] = inputs[i];
        }

        //itterate over all neurons and compute feedforward values 
        for (int i = 1; i < layers.Length; i++)
        {
            for (int j = 0; j < neurons[i].Length; j++)
            {
                float value = 0f;

                for (int k = 0; k < neurons[i - 1].Length; k++)
                {
                    value += weights[i - 1][j][k] * neurons[i - 1][k]; //sum off all weights connections of this neuron weight their values in previous layer
                }

                neurons[i][j] = (float)Math.Tanh(value); //Hyperbolic tangent activation
            }
        }

        return neurons[neurons.Length - 1]; //return output layer
    }


    public void Mutation()
    {
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++) //checking trough all weights
                {
                    float newWeight = weights[i][j][k]; //value to be muteted
                    float spinTheWheel = UnityEngine.Random.Range(0f, 100f);
                    if(spinTheWheel < (mutationChance / 4f)) //one kind of mutation /increase value
                    {
                        float factor = UnityEngine.Random.Range(-0.1f, 0.1f) + 1f;
                        newWeight *= factor;
                    }
                    else if (spinTheWheel < (mutationChance / 2f) && spinTheWheel > (mutationChance / 4f)) //decrease value
                    {
                        float factor = UnityEngine.Random.Range(-0.1f, 0.1f);
                        newWeight *= factor;
                    }
                    else if (spinTheWheel < (mutationChance / 4f * 3f) && spinTheWheel  > (mutationChance / 2f)) //give new random value
                    {
                        newWeight = UnityEngine.Random.Range(-0.5f, 0.5f);
                    }
                    else if (spinTheWheel < (mutationChance) && spinTheWheel > (mutationChance / 4f * 3f)) //flip the value
                    {
                        newWeight *= -1f;
                    }

                    weights[i][j][k] = newWeight;
                }
            }
        }
    }
}
