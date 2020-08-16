using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public Player prefab;
    private GameObject thisPlayer;
    public float size;
    public int bounces = 0;
    public bool changeAppearance = false;
    BoxCollider col;
    public GameObject particles;
    void Start()
    {
        thisPlayer = Instantiate(prefab.appearance,transform.position, transform.rotation);
        size = prefab.size;
        thisPlayer.transform.parent = this.transform;
        col = this.gameObject.AddComponent<BoxCollider>();
        col.size = new Vector3(size, 2f, .5f);
    }
    private void Update()
    {
        if(changeAppearance)
        {
            Destroy(thisPlayer);
            Destroy(col);
            thisPlayer = Instantiate(prefab.appearance, transform.position, transform.rotation);
            size = prefab.size;
            thisPlayer.transform.parent = this.transform;
            col = this.gameObject.AddComponent<BoxCollider>();
            col.size = new Vector3(size, 2f, .5f);
            changeAppearance = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Instantiate(particles, contact.point, Quaternion.identity);
        }
        if (collision.gameObject.name == "Ball")
        {
            bounces++;            
        }
    }
}
