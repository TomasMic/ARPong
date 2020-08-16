using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public bool roundStart = false;
    public float originalSpeed = 10f;
    [SerializeField]
    private float speed;
    public float minSpeedIncrease = 0.005f;
    public Vector3 originalPos;
    Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        //transform.parent = GameObject.Find("Arena").transform;
    }
    // Update is called once per frame
    void Update()
    {
        if (roundStart)
        {
            speed = originalSpeed;
            roundStart = false;
            float random = UnityEngine.Random.Range(0f, 1f);
            if (random > 0.5f) //throw ball on player first
            {
                float randomXFactor = 0f;//UnityEngine.Random.Range(0, 0.5f);
                rb.velocity = new Vector3(speed * randomXFactor, 0f, -speed * (1 - randomXFactor));
                rb.angularVelocity = new Vector3(-speed * (1 - randomXFactor), 0f, speed * randomXFactor);
            }
            else
            {
                float randomXFactor = 0f;// UnityEngine.Random.Range(0, 0.5f);
                rb.velocity = new Vector3(speed * randomXFactor, 0f, speed * (1 - randomXFactor));
                rb.angularVelocity = new Vector3(-speed * (1 - randomXFactor) , 0f, -speed * randomXFactor);
            }
        }
    }
    //comment out for game
    private void FixedUpdate()
    {
        speed += 0.0035f;
    }
    private void OnCollisionEnter(Collision collision)
    {
  
        switch (collision.gameObject.name)
        {
            case "Player" :
                {
                    float size = collision.gameObject.GetComponent<PlayerStats>().size;
                    float newSpeed = ((3f - size) * minSpeedIncrease * collision.gameObject.GetComponent<PlayerStats>().bounces * speed) + speed;
                    float xFactor = (transform.position.x - collision.gameObject.transform.position.x) / (size / 2f) * 0.4f; //if hit on the edge, add velocity at 45deg angle
                    rb.velocity = new Vector3(newSpeed * xFactor, 0f, newSpeed * (1 - xFactor));
                    rb.angularVelocity = new Vector3(-newSpeed * (1 - xFactor), 0f, -newSpeed * xFactor);
                    break;
                }
            case "Computer":
                {
                    float size = collision.gameObject.GetComponent<PlayerStats>().size;
                    float newSpeed = ((3f - size) * minSpeedIncrease * collision.gameObject.GetComponent<PlayerStats>().bounces * speed) + speed;
                    float xFactor = (transform.position.x - collision.gameObject.transform.position.x) / (size / 2f) * 0.4f; //if hit on the edge, add velocity at 45deg angle
                    rb.velocity = new Vector3(newSpeed * xFactor, 0f, -newSpeed * (1 - xFactor));
                    rb.angularVelocity = new Vector3(-newSpeed * (1 - xFactor), 0f, newSpeed * xFactor);
                    break;
                }
            case "AIBody(Clone)":
                {
                    float randomXFactor = UnityEngine.Random.Range(0, 0.5f);
                    rb.velocity = new Vector3(speed * randomXFactor, 0f, -speed * (1 - randomXFactor));
                    rb.angularVelocity = new Vector3(-speed * (1 - randomXFactor), 0f, -speed * randomXFactor);
                    break;
                }
            case "PlayerGoal":
                Destroy(gameObject);
                break;
            case "CompGoal":
                Destroy(gameObject);
                break;

        }
    }
    private void OnDestroy()
    {
        GameObject.Find("Player").GetComponent<PlayerStats>().bounces = 0;
        GameObject.Find("Computer").GetComponent<PlayerStats>().bounces = 0;
    }
}
