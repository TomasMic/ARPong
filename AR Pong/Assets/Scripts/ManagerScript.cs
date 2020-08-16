using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ManagerScript : MonoBehaviour
{
    public int playerScore = 0;
    public int compScore = 0;
    public Text scoreText;
    public bool scored = false;
    public GameObject ball;
    public Text tapText;
    public bool tap = true;
    public GameObject center;
    private bool settingUp = true;
    private int playerSize = 1;
    public Button nextB;
    public Button prevB;
    public Button okB;
    public GameObject settingPanel;
    public GameObject arena;
    private ARRaycastManager rayManager;
    public GameObject ai;
    public bool pcStart = false;
    private void Awake()
    {
        int randomCompAppearance = Mathf.FloorToInt(UnityEngine.Random.Range(1f, 3.9f));
        //Debug.Log(randomCompAppearance);
        GameObject.Find("Computer").GetComponent<PlayerStats>().prefab = (Player)Resources.Load("Objects/Size" + randomCompAppearance.ToString(), typeof(Player));
        Button next = nextB.GetComponent<Button>();
        next.onClick.AddListener(nextSize);
        Button prev = prevB.GetComponent<Button>();
        prev.onClick.AddListener(prevSize);
        Button ok = okB.GetComponent<Button>();
        ok.onClick.AddListener(OK);
        rayManager = FindObjectOfType<ARRaycastManager>();
        //planeManager = GameObject.Find("AR Session Origin").GetComponent<ARPlaneManager>();
    }
    void Update()
    {
        
        if (settingUp)
        {
            tapText.enabled = false;
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            rayManager.Raycast(new Vector2(Screen.width / 2f, Screen.height / 2f), hits, TrackableType.Planes);
            if (hits.Count > 0)
            {
                arena.transform.position = hits[0].pose.position;
                arena.transform.rotation = hits[0].pose.rotation;
            }
            
        }
        else
        {
            
            if (scored)
            {
                scored = false;
                tapText.gameObject.SetActive(true);
                tap = true;
                scoreText.text = playerScore.ToString() + " : " + compScore.ToString();
            }
            if ((Input.touchCount > 0 || pcStart) && tap )
            {
                pcStart = false;
                GameObject newBall = Instantiate(ball, center.transform.position, center.transform.rotation);
                newBall.GetComponent<Ball>().roundStart = true;
                newBall.transform.SetParent(arena.transform);
                ai.GetComponent<GameAI>().ball = newBall;
                tap = false;
                tapText.gameObject.SetActive(false);

            }
        }
    }
    void nextSize()
    {
        if(playerSize != 3)
        {
            playerSize++;
        }
        GameObject.Find("Player").GetComponent<PlayerStats>().changeAppearance = true;
        GameObject.Find("Player").GetComponent<PlayerStats>().prefab = (Player)Resources.Load("Objects/Size" + playerSize.ToString(), typeof(Player));
    }
    void prevSize()
    {
        if(playerSize != 1)
        {
            playerSize--;
        }
        GameObject.Find("Player").GetComponent<PlayerStats>().changeAppearance = true;
        GameObject.Find("Player").GetComponent<PlayerStats>().prefab = (Player)Resources.Load("Objects/Size" + playerSize.ToString(), typeof(Player));
    }
    void OK()
    {
        settingUp = false;
        GameObject.Find("Player").GetComponent<PlayerMovement>().enabled = true;
        tapText.enabled = true;
        settingPanel.SetActive(false);
        
    }
}
