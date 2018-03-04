using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowHidePanel : MonoBehaviour {

    public GameObject Panel;
    int counter;

	// Use this for initialization
	void Start () {
		
	}
    public void ShowHide()
    {
 
        counter++;
        if ( counter % 2 == 1 )
        {
            Panel.gameObject.SetActive(true);
        }
        else
        {
            Panel.gameObject.SetActive(false);
        }

    }
}
