using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLine : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        GameObject.Find("Manager").GetComponent<ManagerScript>().compScore++;
        GameObject.Find("Manager").GetComponent<ManagerScript>().scored = true;
    }
}
