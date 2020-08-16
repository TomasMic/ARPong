using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ManagerAI : MonoBehaviour
{
    [Header("Parameters", order = 0)]
    public int populationSize;
    public int keepTop;
    public float genTime;

    private int generationNumber = 0;
    private int[] layers = new int[] { 7, 21, 14, 5, 1 }; //7 inputs and 1 output
    private List<NeuralNet> nets;
    private List<AI> aIBodies = new List<AI>();
    public Text text;
    public Text score;
    public Text countDownTimer;
    public Toggle showBest;
    public Toggle keepWatching;
    public Button saveToTXT;
    public GameObject origin;
    public GameObject comp;

    private bool isTraning = false;
    private float countdown;
    private float prevAverage = new float();
    float average = new float();
    int indexWatching = 0;
    bool saveNet = false;
    private void Start()
    {
        GameObject.Find("Ball").GetComponent<Ball>().roundStart = true;
        Button btn = saveToTXT.GetComponent<Button>();
        btn.onClick.AddListener(SaveNetToTXT);
        Invoke("CountDown", 1f);
    }

    void Update()
    {
        if (isTraning == false)
        {
            if(saveNet)
            {
                saveNet = false;
                string path = "D:/Desktop/NeuralNet.txt";
                StreamWriter writer = new StreamWriter(path, true);

                float bestScore = nets[0].fitness;
                int bestIndex = 0;

                for (int i = 0; i < populationSize; i++)
                {
                    if (nets[i].fitness > bestScore)
                    {
                        bestIndex = i;
                        bestScore = nets[i].fitness;
                    }
                }

                NeuralNet netToSave = nets[bestIndex];
                for (int i = 0; i < netToSave.weights.Length; i++)
                {
                    for (int j = 0; j < netToSave.weights[i].Length; j++)
                    {
                        for (int k = 0; k < netToSave.weights[i][j].Length; k++)
                        {
                            writer.Write(netToSave.weights[i][j][k].ToString("0.00000") + " ");
                        }
                    }
                }

                writer.Close();
            }
            countdown = genTime;
            GameObject.Find("Ball").GetComponent<Ball>().roundStart = true;
            //showBest.isOn = false;
            
            if (generationNumber == 0)
            {
                InitCompNeuralNetworks();
            }
            else
            {
                SortNets(); //best nets are first
                int indexTocopy = 0;
                for (int i = keepTop; i < populationSize; i++)
                {
                    if(indexTocopy == keepTop + 1)
                    {
                        indexTocopy = 0;
                    }
                    nets[i] = new NeuralNet(nets[indexTocopy]);
                    // int toMutate = keepTop - (int)UnityEngine.Random.Range(0f, (float)keepTop);

                   // nets[i] = nets[toMutate];
                    nets[i].Mutation();
                    
                    nets[indexTocopy] = new NeuralNet(nets[indexTocopy]); //will reset neurons
                    indexTocopy++;
                }
                foreach(NeuralNet n in nets)
                {
                    n.fitness = 0;
                }
            }


            generationNumber++;
            text.text = "Gen: " + generationNumber.ToString();

            isTraning = true;
            Invoke("Timer", genTime);
            
            GenerateAIBodies();

            
        }
    }

    private void FixedUpdate()
    {
        if (isTraning)
        {
            float bestScore = nets[0].fitness;
            int bestIndex = 0;
            average = 0;

            for (int i = 0; i < populationSize; i++)
            {
                if (nets[i].fitness > bestScore)
                {
                    bestIndex = i;
                    bestScore = nets[i].fitness;
                }
                average += nets[i].fitness;
            }

            if (showBest.isOn == true && keepWatching.isOn == false)
            {
                indexWatching = bestIndex;
                foreach (AI a in aIBodies)
                {
                    if (a.index != bestIndex)
                    {
                        a.GetComponent<MeshRenderer>().enabled = false;
                    }
                    else
                    {
                        a.GetComponent<MeshRenderer>().enabled = true;
                    }
                }
            }
            else if (showBest.isOn == true && keepWatching.isOn == true)
            {
                foreach (AI a in aIBodies)
                {
                    if (a.index != indexWatching)
                    {
                        a.GetComponent<MeshRenderer>().enabled = false;
                    }
                    else
                    {
                        a.GetComponent<MeshRenderer>().enabled = true;
                    }
                }
            }
            else
            {
                foreach(AI a in aIBodies)
                {
                    a.GetComponent<MeshRenderer>().enabled = true;

                }
                keepWatching.isOn = false;
            }
            
            score.text = "Average Score: " + (average / populationSize).ToString("0.0") + " " + (average > prevAverage ? '↑' : '↓') + "\nBest Score: " + bestScore.ToString("0.0") + "\nat index" + bestIndex.ToString();
        }
    }

    void SortNets()
    {
        for (int i = 0; i < nets.Count - 1; i++)
        {
            for (int j = i; j < nets.Count; j++)
            {
                if(nets[j].fitness > nets[i].fitness)
                {
                    NeuralNet temp = nets[i];
                    nets[i] = nets[j];
                    nets[j] = temp;
                }
            }
        }
    }

    void Timer()
    {
        isTraning = false;
        prevAverage = average;
        
    }

    void CountDown()
    {
        countdown -= 1f;
        countDownTimer.text = "Next genocide in: " + countdown.ToString("0") + "s";
        Invoke("CountDown", 1f);
    }
    private void GenerateAIBodies()
    {
        if (aIBodies != null)
        {
            for (int i = 0; i < aIBodies.Count; i++)
            {
                GameObject.Destroy(aIBodies[i].gameObject);
            }

        }

        aIBodies = new List<AI>();

        for (int i = 0; i < populationSize; i++)
        {
            nets[i].fitness = 0;
            AI ai = Instantiate(comp.GetComponent<AI>());
            ai.Init(nets[i]);
            ai.transform.parent = origin.transform;
            ai.index = i;
            ai.score = 0;
            aIBodies.Add(ai);
        }

    }

    void InitCompNeuralNetworks()
    {

        nets = new List<NeuralNet>();


        for (int i = 0; i < populationSize; i++)
        {
            NeuralNet net = new NeuralNet(layers);
            //net.Mutation();
            nets.Add(net);
        }
    }

    void SaveNetToTXT()
    {
        saveNet = true;
        
    }
}
