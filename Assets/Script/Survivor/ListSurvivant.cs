using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListSurvivant : MonoBehaviour {

    private List<Survivor> survivors;

    public GameObject panel;

    Text completName, mental, physic;
    int[] mentalChangement;
    int[] physicChangement;

    // Use this for initialization
    void Start () {
        
        survivors = SurvivorManager.GetInstance().Survivors;
        mentalChangement = SurvivorManager.GetInstance().MentalIncreaser;
        physicChangement = SurvivorManager.GetInstance().PhysicIncreaser;


        for (int i = 0; i < survivors.Count; i++)
        {
            // Instantiate several panel and change color in function of the mental/physics stats

            GameObject newPanel = Instantiate(panel, GetComponent<Transform>());
            newPanel.GetComponent<RectTransform>().position = new Vector2(newPanel.GetComponent<RectTransform>().position.x, newPanel.GetComponent<RectTransform>().position.y - i * 90);
            newPanel.transform.GetChild(0).GetComponent<Text>().text = survivors[i].FirstName + ' ' + survivors[i].LastName;
            if (survivors[i].MentalHealth >= 0 && survivors[i].MentalHealth < 34)
            {
                newPanel.transform.GetChild(3).GetComponent<Text>().color = Color.red;
            }
            if (survivors[i].MentalHealth >= 34 && survivors[i].MentalHealth < 67)
            {
                newPanel.transform.GetChild(3).GetComponent<Text>().color = new Color(1.0F, 0.5F, 0.31F, 1.0F);
            }
            if (survivors[i].MentalHealth >= 67 && survivors[i].MentalHealth < 101)
            {
                newPanel.transform.GetChild(3).GetComponent<Text>().color = Color.green;
            }
            newPanel.transform.GetChild(3).GetComponent<Text>().text = survivors[i].MentalHealth.ToString();

            if (survivors[i].MentalHealth >= 0 && survivors[i].MentalHealth < 34)
            {
                newPanel.transform.GetChild(4).GetComponent<Text>().color = Color.red;  // works well
            }
            if (survivors[i].MentalHealth >= 34 && survivors[i].MentalHealth < 67)
            {
                newPanel.transform.GetChild(4).GetComponent<Text>().color = new Color(1.0F, 0.5F, 0.31F, 1.0F);
            }
            if (survivors[i].MentalHealth >= 67 && survivors[i].MentalHealth < 101)
            {
                newPanel.transform.GetChild(4).GetComponent<Text>().color = Color.green;
            }
            newPanel.transform.GetChild(4).GetComponent<Text>().text = survivors[i].PhysicalHealth.ToString();

            // New mental statistics added
            if (survivors[i].MentalChangement != 0)
            {
                if (survivors[i].MentalChangement > 0)
                {
                    newPanel.transform.GetChild(5).GetComponent<Text>().color = Color.green;
                    newPanel.transform.GetChild(5).GetComponent<Text>().text = "+" + survivors[i].mentalChange.ToString();
                }
                else
                {
                    if (survivors[i].MentalChangement < 0)
                    {
                        newPanel.transform.GetChild(5).GetComponent<Text>().color = Color.red;
                        newPanel.transform.GetChild(5).GetComponent<Text>().text = survivors[i].mentalChange.ToString();
                    }
                }
            }
            else
            {
                newPanel.transform.GetChild(5).GetComponent<Text>().color = Color.green;
                newPanel.transform.GetChild(5).GetComponent<Text>().text = "+" + survivors[i].mentalChange.ToString();
            }

            // New physics statistics added
            if (survivors[i].PhysicalChangement != 0)
            {
                if (survivors[i].PhysicalChangement > 0)
                {
                    newPanel.transform.GetChild(6).GetComponent<Text>().color = Color.green;
                    newPanel.transform.GetChild(6).GetComponent<Text>().text = "+" + survivors[i].physicalChange.ToString();
                }
                else
                {
                    if (survivors[i].PhysicalChangement < 0)
                    {
                        newPanel.transform.GetChild(6).GetComponent<Text>().color = Color.red;
                        newPanel.transform.GetChild(6).GetComponent<Text>().text = survivors[i].physicalChange.ToString();
                    }
                }
            }
            else
            {
                newPanel.transform.GetChild(6).GetComponent<Text>().color = Color.green;
                newPanel.transform.GetChild(6).GetComponent<Text>().text = "+" + survivors[i].physicalChange.ToString();
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
