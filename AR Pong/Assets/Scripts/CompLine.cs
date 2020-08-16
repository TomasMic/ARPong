using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompLine : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        GameObject.Find("Manager").GetComponent<ManagerScript>().playerScore++;
        GameObject.Find("Manager").GetComponent<ManagerScript>().scored = true;
    }
}
