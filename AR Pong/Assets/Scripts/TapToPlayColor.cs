using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapToPlayColor : MonoBehaviour
{
    Text text;
    float t = 0;
    bool atMaximum = false;
    bool atMinimum = true;
    void Awake()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        text.color = Color.Lerp(Color.blue, Color.red, t);
        if (atMinimum)
        {
            t += 0.01f;
            if (t >= 1f)
            {
                atMaximum = true;
                atMinimum = false;
            }
        }
        if (atMaximum)
        {
            t -= 0.01f;
            if(t <= 0f)
            {
                atMaximum = false;
                atMinimum = true;
            }
        }

    }
}
