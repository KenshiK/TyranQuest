using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceTextDisplay : MonoBehaviour {

    protected Village village;
    protected Text text;
    // Use this for initialization
	void Start () {
        village = Village.GetInstance();
        text = GetComponent<Text>();
	}
	
	
}
