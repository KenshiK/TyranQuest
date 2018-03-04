using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playSoundOnClick : MonoBehaviour {

    public AudioSource audioSource;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Onclick()
    {
        audioSource.Play();
    }
}
