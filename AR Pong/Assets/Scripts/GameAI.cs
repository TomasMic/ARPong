using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class GameAI : MonoBehaviour
{
    public GameObject ball;
    float size;
    private NeuralNet net;
    public float speed;
    float prevZballPos = 0f;
    float prevXballPos = 0f;
    int[] layers = new int[] { 7, 21, 14, 5, 1 };
    public TextAsset neuralNetString;

    private void Awake()
    {
        int index = 0;
        
        
        size = GetComponent<PlayerStats>().size;
        List<float> weights = Weights();
        net = new NeuralNet(layers);
        for (int i = 0; i < net.weights.Length; i++)
        {
            for (int j = 0; j < net.weights[i].Length; j++)
            {
                for (int k = 0; k < net.weights[i][j].Length; k++)
                {
                    net.weights[i][j][k] = weights[index];
                    index++;
                }
            }
        }
    }
    void FixedUpdate()
    {
        float distanceX;
        float distanceZ;
        if (ball != null)
        {
            distanceX = ball.transform.localPosition.x - transform.localPosition.x;
            distanceZ = ball.transform.localPosition.z - transform.localPosition.z;
            float[] inputs = new float[] { distanceX, prevXballPos, distanceZ, prevZballPos, size };
            prevZballPos = distanceZ;
            prevXballPos = distanceX;
            float[] output = net.FeedForward(inputs);
            transform.Translate(output[0] * Vector3.right * speed * Time.fixedDeltaTime, Space.Self);
            //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        }     
    }

    List<float> Weights ()
    {
        List<float> weightsList = new List<float>();
        string s = neuralNetString.ToString();
        string resString = "";
        foreach (char ch in s)
        {            
            if(ch != ' ')
            {
                resString += ch;
            }
            else
            {
                float result;
                float.TryParse(resString, out result);
                weightsList.Add(result);
                resString = "";
            }
        }
        return weightsList;
    }
}
