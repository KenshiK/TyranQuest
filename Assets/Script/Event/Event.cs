using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Event : MonoBehaviour {

    public string text;
    public Dialogue dialogue;
    public Choice[] choices;

    [Header("Deviant behavior Settings")]
    public Choice[] deviantChoices;
    public float luck = 0.25f;

    [Header("Advanced Settings")]
    [SerializeField] protected int weight = 5;
    public int baseWeight = 5;
    [SerializeField] protected bool happenOnce;
    public Condition eventConditions;
    public int Weight { get { return weight; } set { weight = Mathf.Max(0, value); } }

    // Use this for initialization
    void Start()
    {
        DialogueManager.GetInstance().StartDialogue(dialogue);

        /*
         * REFACT
         */
        //textWindowManager.GetInstance().AddToDico(4, dialogue.sentences);
    }

	// Update is called once per frame
	void Update () {
	}

    public void ApplyChoice(int num=0)
    {
        Choice c;
        Village village = Village.GetInstance();
        if (choices.Length > 1)
        {
            float rnd = Random.Range(0, 1);
            if (deviantChoices.Length > 0 && rnd < luck && deviantChoices.Length >= num + 1)
                c = deviantChoices[num];
            else
                c = choices[num];

            village.UpdateVillage(c);
        }
        else
        {
            SurvivorManager survivorManager = SurvivorManager.GetInstance();
            c = choices[num];

            survivorManager.EffectOnSurvivors(c.PhysicalEffect, c.MentalEffect, c.WholeVillage);
            village.ChangePopulation(c.PopulationEffect);
            survivorManager.CheckSurvivorsHealth();
            village.UpdateAuthority();

            GameObject Currentbutton = village.HUDEvent.transform.GetChild(0).gameObject;
            Currentbutton.GetComponentInChildren<Text>().text = this.choices[0].text;
            Currentbutton.GetComponent<Button>().onClick.RemoveAllListeners();
            Currentbutton.GetComponent<Button>().onClick.AddListener(delegate { this.GetComponent<Event>().ApplyChoice(0); });
        }
    }
    
}
