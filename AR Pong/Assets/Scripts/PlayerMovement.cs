using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Camera cam;
    public GameObject floor;
    MeshFilter mFloor;
    // Start is called before the first frame update
    void Start()
    {
        mFloor = floor.GetComponent<MeshFilter>();
    }

    // Update is called once per frame
    void Update()
    {
        float size = GetComponent<PlayerStats>().size;
        float arenaSize = 15f;
        float clamp = arenaSize / 2f + size / 2f;

        transform.position = new Vector3(cam.transform.position.x, transform.position.y, transform.position.z);

        //transform.position = new Vector3(Mathf.Clamp(Input.mousePosition.x, -clamp, clamp), transform.position.y, transform.position.z);
    }
}
