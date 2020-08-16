using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    private bool initilized = false;
    public GameObject ball;
    float size;
    private NeuralNet net;
    public float speed;
    float prevZballPos = 0f;
    float prevXballPos = 0f;
    public int index;
    public float score;

    private void Start()
    {
        ball = GameObject.Find("Ball");
    }
    void FixedUpdate()
    {
        float changeSize = UnityEngine.Random.Range(0f, 500f);
        if (changeSize < 1f)
        {
            float newSize = UnityEngine.Random.Range(1f, 3f);
            if(newSize <= 1f)
            {
                size = 1f;
            }
            else if (newSize <= 2f && newSize > 1f)
            {
                size = 2f;
            }
            else if (newSize <= 3f && newSize > 2f)
            {
                size = 3f;
            }
            transform.localScale = new Vector3(size, 1f, 1f);
        }

        if (initilized)
        {
            float distanceX = ball.transform.localPosition.x - transform.localPosition.x;
            float distanceZ = ball.transform.localPosition.z - transform.localPosition.z;
            float[] inputs = new float[] { distanceX, prevXballPos, distanceZ, prevZballPos, size, transform.localPosition.x, 5f }; //5f is half of the arena size, there are 2 memmory cells
            prevZballPos = distanceZ;
            prevXballPos = distanceX;
            float[] output = net.FeedForward(inputs);

            transform.Translate(output[0] * Vector3.left * speed * Time.fixedDeltaTime, Space.Self);
            //transform.position = new Vector3(Mathf.Clamp(transform.position.x, -7.5f + size / 2f, 7.5f - size / 2f), transform.position.y, transform.position.z);
            if (Mathf.Abs(distanceX) > size/2f)
            {
                net.fitness -= 0.01f;
            }
            else
            {
                net.fitness += 0.01f;
            }                     
        }
        score = net.fitness;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Ball")
        {
            net.fitness += 20f;
        }
    }
    public void Init(NeuralNet net)
    {
        this.net = net;
        initilized = true;
    }
}


