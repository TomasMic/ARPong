using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bounceBottom : MonoBehaviour
{
    float velocityToAdd = 1f;
    private void OnCollisionEnter(Collision collision)
    {
        float randomXFactor = UnityEngine.Random.Range(0, 0.7f);
        collision.gameObject.GetComponent<Rigidbody>().velocity += new Vector3(velocityToAdd * randomXFactor, 0f, velocityToAdd * (1 - randomXFactor));

    }
}
