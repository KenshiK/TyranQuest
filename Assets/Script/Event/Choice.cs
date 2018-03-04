using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Choice {
    public string text;
    [SerializeField] protected int physicalEffect;
    [SerializeField] protected int mentalEffect;
    [SerializeField] protected int populationEffect;
    public bool WholeVillage = false;
    public Condition choiceConditions;
    public ChildEvent childEvent;

    public Choice(int PE, int ME, int pop, string txt)
    {
        PhysicalEffect = PE;
        MentalEffect = ME;
        PopulationEffect = pop;
        text = txt;
    }

    public Choice()
    {

    }

    public int PhysicalEffect
    {
        get
        {
            return physicalEffect;
        }
        set
        {
            physicalEffect = value;
        }
    }

    public int MentalEffect
    {
        get
        {
            return mentalEffect;
        }
        set
        {
            mentalEffect = value;
        }
    }

    public int PopulationEffect
    {
        get
        {
            return populationEffect;
        }
        set
        {
            populationEffect = value;
        }
    }

}
