using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TenteController : MonoBehaviour {



    Transform[] tabTente;
    SurvivorManager survivorManager;
    int prevPop = 0;

    public GameObject explosion;
    public AudioSource exploSound;

	// Use this for initialization
	void Start () {
        survivorManager = SurvivorManager.GetInstance();
        tabTente = GetComponentsInChildren<Transform>();
    }
	
	// Update is called once per frame
	void Update () {
        int actualPop = survivorManager.Survivors.Count;
        if(actualPop < prevPop)
        {
            StartCoroutine(MakeSoundExplosion());
        }

        if(actualPop != prevPop)
        {
            //En fait reapparaitre un nombre égale au nombre d'habitant
            for (int i = 0; i < tabTente.Length; i++)
            {
                //Si la tente est une des tentes qui a été detruite alors on la fait peter dans 5 secondes (apres le fade)
                if(actualPop < prevPop && i != 0 && i > actualPop)
                {
                    StartCoroutine(MakeExplosion(tabTente[i]));
                    tabTente[i].gameObject.SetActive(false);
                }
                else if(i <= actualPop)
                {
                    tabTente[i].gameObject.SetActive(true);
                } else
                {
                    tabTente[i].gameObject.SetActive(false);
                }
            }

            if (actualPop < tabTente.Length)
                prevPop = actualPop;
            else
                prevPop = tabTente.Length;

        }

    }

    IEnumerator MakeExplosion(Transform t)
    {
        yield return new WaitForSeconds(2);
        GameObject tent = Instantiate(explosion, null);
        tent.GetComponent<Transform>().position = t.position;
    }

    IEnumerator MakeSoundExplosion()
    {
        yield return new WaitForSeconds(2);
        exploSound.Play();
    }
}
