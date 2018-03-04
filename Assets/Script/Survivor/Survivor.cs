using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Survivor {
    const int maxResource = 100;
    const int minResource = 0;
    private string firstName;
    private string lastName;
    private int mentalHealth;
    private int physicalHealth;
    public int mentalChange = 0;
    public int physicalChange = 0;
    private int id;
    public Survivor assassin = null;

    public Survivor(string fName, string lName, int mHealth, int pHealth, int num)
    {
        firstName = fName;
        lastName = lName;
        mentalHealth = mHealth;
        physicalHealth = pHealth;
        id = num;
    }
	public void ChangeMentalHealth(int change)
    {
        mentalHealth = Mathf.Min(Mathf.Max(mentalHealth + change, minResource), maxResource);
        mentalChange = change;
    }

    public void ChangePhysicalHealth(int change)
    {
       physicalHealth = Mathf.Min(Mathf.Max(physicalHealth + change, minResource), maxResource);
       physicalChange = change;
    }

    public void Assassinate(Survivor victim)
    {
        victim.assassin = this;
        victim.physicalHealth = 0;
    }
    public string FirstName
    {
        get
        {
            return firstName;
        }
    }
    public string LastName
    {
        get
        {
            return lastName;
        }
    }
    public int MentalHealth
    {
        get
        {
            return mentalHealth;
        }
    }
    public int PhysicalHealth
    {
        get
        {
            return physicalHealth;
        }
    }
    public int Id
    {
        get
        {
            return id;
        }
        set
        {
            id = value;
        }
    }
    
    public int MentalChangement
    {
        get
        {
            return mentalChange;
        }
    }

    public int PhysicalChangement
    {
        get
        {
            return physicalChange;
        }
    }

    

    public void ChangeResource(int physical, int mental)
    {
        ChangePhysicalHealth(physical);
        ChangeMentalHealth(mental);
    }
	
}
