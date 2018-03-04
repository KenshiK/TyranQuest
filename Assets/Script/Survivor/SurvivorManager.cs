using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorManager : MonoBehaviour {
    public static SurvivorManager instance;
    [SerializeField] private int survivorCount = 5;
    [SerializeField] private TextAsset nameList;
    [SerializeField] private int lowerBound = 30;
    [SerializeField] private int upperBound = 70;
    [SerializeField] private int newMemberMentalImpact = 20;
    [SerializeField] private int maxPopulation = 10;
    private string[] names;
    private int survivorId = 0;
    private List<Survivor> survivors = new List<Survivor>();
    private Village village;
    //private int overallSatisfaction;

    private int[] mentalIncrease;
    private int[] physicIncrease;

    public List<Survivor> Survivors
    {
        get
        {
            return survivors;
        }
    }
    public static SurvivorManager GetInstance()
    {
        if (instance == null)
        {
            Debug.Log("No instance of " + typeof(SurvivorManager));
            return null;
        }
        return instance;
    }
    public int DeathMentalImpact
    {
        get
        {
            return (int)((1f / survivorCount) * -100f);
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        names = nameList.text.Split('\n');
        for (int i = 0; i < survivorCount; i++)
        {
            string fName = names[Random.Range(0, names.Length)];
            string lName = names[Random.Range(0, names.Length)];
            int mHealth = Random.Range(lowerBound, upperBound);
            int pHealth = Random.Range(lowerBound, upperBound);
            int id = survivorId;
            survivorId++;
            Survivor temp = new Survivor(fName, lName, mHealth, pHealth, id);
            survivors.Add(temp);
        }
    }

    private void Start()
    {
        village = Village.GetInstance();
        
    }

    public void EffectOnSurvivors(int physical, int mental, bool whole = false)
    {
        if (whole)
        {
            foreach(Survivor s in survivors)
            {
                s.ChangeResource(physical, mental);

                s.mentalChange = mental;
                s.physicalChange = physical;
            }
        }
        else
        {
            int numberAffected = Random.Range(1, survivors.Count); // affected survivors are chosen randomly
            List<int> temp = new List<int>();
            int bound = numberAffected > survivors.Count ? survivors.Count : numberAffected;
            for (int i = 0; i < bound; i++)
            {
                
                int k = Random.Range(0, survivors.Count);
                if (!temp.Contains(k))
                {
                    temp.Add(k);
                }
                else if(temp.Count < 1 && i == bound - 1)
                {
                    i--;
                }
                    
            }
            foreach(int j in temp)
            {
                survivors[j].ChangeResource(physical, mental);
                
            }
        }
    }

    public void CheckSurvivorsHealth()
    {
        List<Survivor> temp = new List<Survivor>();
        foreach(Survivor s in survivors)
        {
            if(s.MentalHealth < lowerBound)
            {
                int rnd = Random.Range(0, 100);
                if(rnd <= 50)
                {
                    Survivor victim = survivors[Random.Range(0, survivors.Count)];
                    s.Assassinate(victim);
                    temp.Add(victim);
                }
            }
            if(s.PhysicalHealth == 0)
            {
                if (!temp.Contains(s))
                {
                    temp.Add(s);
                }
            }
        }
        if (temp.Count > 0)
        {
            for (int i = temp.Count - 1; i >= 0; i--)
            {
                KillSurvivor(temp[i]);
            }
        }
        
    }

    public void UpdateOverallSatisfaction()
    {

        foreach(Survivor s in survivors)
        {
            village.Authority -= (s.MentalHealth - 50) / 10;
            village.Authority -= (s.PhysicalHealth - 50) / 10;
        }
            /*
            for(int i = 0; i < survivors.Count; i++)
            {
                Survivor temp = survivors[i];
                if(temp.PhysicalHealth >= upperBound || temp.PhysicalHealth <= lowerBound)
                {
                    overallPhysical += Mathf.RoundToInt(temp.PhysicalHealth * thresholdMultiplicator);
                }
                else
                {
                    overallPhysical += temp.PhysicalHealth;
                }
                if (temp.MentalHealth >= upperBound || temp.MentalHealth <= lowerBound)
                {
                    overallMental += Mathf.RoundToInt(temp.MentalHealth * thresholdMultiplicator);
                }
                else
                {
                    overallMental += temp.MentalHealth;
                }
            }
            overallPhysical = (int)Mathf.Floor(overallPhysical / survivors.Count); ;
            overallMental = (int)Mathf.Floor(overallMental / survivors.Count);

            overallSatisfaction = Mathf.Abs(overallPhysical - 50) > Mathf.Abs(overallMental - 50) ? overallPhysical : overallMental;*/
    }

    public void AddSurvivor(int number)
    {
        for(int i = 0; i < number; i++)
        {
            if (survivors.Count + 1 > maxPopulation)
                break;
            foreach (Survivor remaining in survivors)
            {
                remaining.ChangeMentalHealth(newMemberMentalImpact);
            }
            string fName = names[Random.Range(0, names.Length)];
            string lName = names[Random.Range(0, names.Length)];
            int mHealth = Random.Range(lowerBound, upperBound);
            int pHealth = Random.Range(lowerBound, upperBound);
            int id = survivorId;
            survivorId++;
            Survivor temp = new Survivor(fName, lName, mHealth, pHealth, id);
            survivors.Add(temp);
        }
        
    }

    public void KillSurvivor(Survivor s)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        if(s.assassin != null)
        {
            if(s.assassin == s)
            {
                sb.Append(s.FirstName).Append(" ").Append(s.LastName).Append(" commited suicide");
            }
            else if(s.assassin != null)
            {
                sb.Append(s.FirstName).Append(" ").Append(s.LastName).Append(" was killed by ").Append(s.assassin.FirstName).Append(" ").Append(s.assassin.LastName);
            }
            else
            {
                sb.Append(s.FirstName).Append(" ").Append(s.LastName).Append(" died");
            }
        }
        else
            sb.Append(s.FirstName).Append(" ").Append(s.LastName).Append(" died");
        Debug.Log(sb.ToString());
        string writeToDialogue = sb.ToString();
        //DialogueManager.GetInstance().StartDialogue(new Dialogue(new string[] { writeToDialogue }));

        /*
         * REFACT 
         */
        //textWindowManager.GetInstance().AddToDico(2, writeToDialogue);

        survivors.Remove(s);
        foreach (Survivor remaining in survivors)
        {

            remaining.ChangeMentalHealth(DeathMentalImpact);
        }
    }
   
    
    public int[] MentalIncreaser
    {
        get
        {
            return mentalIncrease;
        }
    }

    public int[] PhysicIncreaser
    {
        get
        {
            return physicIncrease;
        }
    }

    /*
    public int OverallSatisfaction
    {
        get
        {
            return overallSatisfaction;
        }
    }*/
}
