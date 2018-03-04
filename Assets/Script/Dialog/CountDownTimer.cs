using System.Collections;
using UnityEngine;

public class CountDownTimer : MonoBehaviour {



    // Use this when awake
    private void Awake()
    {
        
    }

    float timeLeft = 30.0f;

    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            //GameOver();
        }
    }
}
