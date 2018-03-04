using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Village : MonoBehaviour
{
    public const int maxResources = 100;
    public const int minResources = 0;

    public GameObject survivorsPanel;
    public GameObject eventPanel;
    private bool eventIsActive = true;

    private static Village instance = null;
    private SurvivorManager survivorManager = null;

    [SerializeField] private GameObject[] possibleEvents;
    [SerializeField] private GameObject[] possibleSecondaryEvents;
    [SerializeField] private GameObject HUD;
    public GameObject HUDEvent;
    public string physicalGainText;
    public string physicalLossText;
    public string mentalGainText;
    public string mentalLossText;
    public string populationGainText;
    public string populationlLossText;
    [SerializeField] private DayAnnouncer dayAnnouncer;
    private Survivor[] survivors;

    private GameObject currentEvent = null;
    private GameObject lastEvent = null;
    private Choice currentChoice = null;
    public Choice CurrentChoice
    {
        get
        {
            return currentChoice;
        }
    }
    private int population, authority;
    private int daysSurvived;
    private int tempPop;
    private int score;

    public RectTransform authTransform;
    private float currentAuth;

    public int maxAuth;
    public Text authText;
    public Image visualAuth;

    public static Village GetInstance()
    {
        if (instance == null)
        {
            Debug.Log("No instance of " + typeof(Village));
            return null;
        }
        return instance;
    }
    public int Population
    {
        get
        {
            return population;
        }
        set
        {
            population = value;
        }
    }
    public int Authority
    {
        get
        {
            return authority;
        }
        set
        {
            authority = Mathf.Min(Mathf.Max(value, minResources), maxResources);
        }
    }
    public int DaysSurvived
    {
        get
        {
            return daysSurvived;
        }
    }

    GameObject newPanelSurvivor = null;

    public DialogueManager dialogueManager;

    // Use this for initialization
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(new Vector3(transform.position.x, transform.position.y, 0) + transform.position, new Vector3(3, 3, 1));
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
    }
    void Start()
    {
        survivorManager = SurvivorManager.GetInstance();
        authority = 50;
        daysSurvived = 0;
        score = 0;

        DeleteChoiceHUD();
        
        StartCoroutine(dayAnnouncer.DisplayDay());
        //dialogueManager = DialogueManager.GetInstance();
        eventPanel.SetActive(true);

        CurrentAuth = maxAuth;
    }

    // Update is called once per frame
    void Update()
    {
        tempPop = survivorManager.Survivors.Count;

        authText.text = "Authority :" + CurrentAuth;
        visualAuth.fillAmount = currentAuth / 100;
    }

    private void CheckGameOver()
    {
        if(authority == 0)
        {
            dayAnnouncer.gameStatus = Enum_GameOver.lowAuthority;
        }
        if(authority == 100)
        {
            dayAnnouncer.gameStatus = Enum_GameOver.highAuthority;
        }
        if(survivorManager.Survivors.Count == 0)
        {
            dayAnnouncer.gameStatus = Enum_GameOver.death;
        }
    }
    public void ChangePopulation(int change)
    {
        if (change < 0)
        {
            change = Mathf.Abs(change);
            for (int i = 0; i < change; i++)
            {
                List<Survivor> temp = survivorManager.Survivors;
                if (temp.Count > 0)
                {
                    int index = Random.Range(0, temp.Count);
                    survivorManager.KillSurvivor(temp[index]);
                }
                else
                {
                    Debug.Log("no one left to kill");
                    break;
                }
            }
        }
        else if (change > 0)
        {
            survivorManager.AddSurvivor(change);
        }
    }

    public void UpdateAuthority()
    {
        survivorManager.UpdateOverallSatisfaction();
    }

    public void UpdateScore()
    {
        score += survivorManager.Survivors.Count * daysSurvived;
    }
    

    public void UpdateVillage(Choice c)
    {
        daysSurvived++;
        UpdateScore();
        currentChoice = c;
        StartCoroutine(dayAnnouncer.DisplayDay());
    }

    public IEnumerator VillageCoroutine()
    {
        yield return null;
        if (currentChoice != null)
            Results();

        survivorManager.EffectOnSurvivors(currentChoice.PhysicalEffect, currentChoice.MentalEffect, currentChoice.WholeVillage);
        ChangePopulation(currentChoice.PopulationEffect);
        survivorManager.CheckSurvivorsHealth();
        UpdateAuthority();
        CheckGameOver();
        ChooseEvent();
    }

    private float CurrentAuth
    {
        get
        {
            return currentAuth;
        }

        set
        {
            currentAuth = Authority;
        }
    }

    public int Score
    {
        get
        {
            return score;
        }
    }

    public void ChooseEvent()
    {
        currentEvent = null;

        // Applique les effets du childEvent du choix
        if (currentChoice != null)
        {
            if (currentChoice.childEvent.childObject != null)
            {
                currentChoice.childEvent.childObject.GetComponent<Event>().Weight += currentChoice.childEvent.childWeightChange;
                if (currentChoice.childEvent.childHappenNext)
                    currentEvent = currentChoice.childEvent.childObject;
            }
        }

        // Choisis un évènement aléatoire
        while (currentEvent == null)
        {
            currentEvent = RandomEvent(possibleEvents);
            Condition c = currentEvent.GetComponent<Event>().eventConditions;
            if (!c.AuthorityCheck())
                currentEvent = null;
        }

        // Applique les effets du childEvent du choix partie 2
        if (currentChoice != null)
        {
            if (currentChoice.childEvent.childNextTurnReset)
            {
                currentChoice.childEvent.childObject.GetComponent<Event>().Weight = currentChoice.childEvent.childObject.GetComponent<Event>().baseWeight;
            }
        }

        if (lastEvent != null)
            Destroy(lastEvent);

        lastEvent = Instantiate(currentEvent);

        UpdateChoiceHUD();
    }

    public void UpdateChoiceHUD()
    {
        DeleteChoiceHUD();

        StartCoroutine("Setter");
    }

    protected IEnumerator Setter()
    {
        yield return new WaitForSecondsRealtime(1f);
        /*
         * REFACT
         */
        /*yield return StartCoroutine(textWindowManager.GetInstance().DisplaySentences());
        Debug.Log("Out");*/
        SetChoiceHUD();
    }

    public void DeleteChoiceHUD()
    {
        for (int i = 0; i < HUDEvent.transform.childCount; i++)
        {
            HUDEvent.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void SetChoiceHUD()
    {
        GameObject Currentbutton;
        for (int i = 0; i < currentEvent.GetComponent<Event>().choices.Length; i++)
        {
            if (currentEvent.GetComponent<Event>().choices[i].choiceConditions.FullCheck())
            {

                Currentbutton = HUDEvent.transform.GetChild(i).gameObject;
                Currentbutton.SetActive(true);
                Currentbutton.GetComponentInChildren<Text>().text = currentEvent.GetComponent<Event>().choices[i].text;
                Currentbutton.GetComponent<Button>().onClick.RemoveAllListeners();
                int temp = i;
                Currentbutton.GetComponent<Button>().onClick.AddListener(delegate { currentEvent.GetComponent<Event>().ApplyChoice(temp); });

                
            }
            //dialogueManager.DisplayNextSentence();
        }
        /*
        GameObject Currentbutton;
        for (int i = 0; i < currentEvent.GetComponent<Event>().choices.Length; i++)
        {
            if (currentEvent.GetComponent<Event>().choices[i].choiceConditions.FullCheck())
            {
                Currentbutton = HUDEvent.transform.GetChild(i).gameObject;
                Currentbutton.SetActive(true);
                Currentbutton.GetComponentInChildren<Text>().text = currentEvent.GetComponent<Event>().choices[i].text;
                Currentbutton.GetComponent<Button>().onClick.RemoveAllListeners();
                int temp = i;
                Currentbutton.GetComponent<Button>().onClick.AddListener(delegate { currentEvent.GetComponent<Event>().ApplyChoice(temp); });
            }
        }*/
    }

    public void Results()
    {
        string[] resultSentences = null;
        System.Text.StringBuilder physique = new System.Text.StringBuilder();
        System.Text.StringBuilder mental = new System.Text.StringBuilder();
        System.Text.StringBuilder population = new System.Text.StringBuilder();

        Queue<System.Text.StringBuilder> sb_list = new Queue<System.Text.StringBuilder>();

        if (currentChoice.PhysicalEffect > 0)
        {
            //physique.Append(gainText).Append(currentChoice.PhysicalEffect).Append(" en physique");
            physique.Append(physicalGainText);
            sb_list.Enqueue(physique);
        }
        else if (currentChoice.PhysicalEffect < 0)
        {
            //physique.Append(lossText).Append(currentChoice.PhysicalEffect).Append(" en physique");
            physique.Append(physicalLossText);
            sb_list.Enqueue(physique);
        }

        if (currentChoice.MentalEffect > 0)
        {
            //mental.Append(gainText).Append(currentChoice.MentalEffect).Append(" en mental");
            mental.Append(mentalGainText);
            sb_list.Enqueue(mental);
        }
        else if (currentChoice.MentalEffect < 0)
        {
            //mental.Append(lossText).Append(currentChoice.MentalEffect).Append(" en mental");
            mental.Append(mentalLossText);
            sb_list.Enqueue(mental);
        }

        if (currentChoice.PopulationEffect > 0)
        {
            //population.Append(gainText).Append(currentChoice.PopulationEffect).Append(" en population");
            population.Append(physicalGainText);
            sb_list.Enqueue(population);
        }
        else if (currentChoice.PopulationEffect < 0)
        {
            //population.Append(lossText).Append(currentChoice.PopulationEffect).Append(" en population");
            population.Append(physicalLossText);
            sb_list.Enqueue(population);
        }

        if (sb_list.Count > 0)
        {
            resultSentences = new string[sb_list.Count];
            for (int l = 0; l < sb_list.Count; l++)
            {
                resultSentences[l] = sb_list.Dequeue().ToString();
            }
        }

        if (possibleSecondaryEvents.Length > 0)
        {
            GameObject secondEvent = RandomEvent(possibleSecondaryEvents);

            if (resultSentences != null)
            {
                dialogueManager.StartDialogue(new Dialogue(resultSentences));
                /*
                 * REFACT
                 */
                //textWindowManager.GetInstance().AddToDico(1, resultSentences);
            }


            //Choice c = secondEvent.GetComponent<Event>().choices[0];

            secondEvent.GetComponent<Event>().ApplyChoice();
        }
        /*
        survivorManager.EffectOnSurvivors(c.PhysicalEffect, c.MentalEffect, c.WholeVillage);
        ChangePopulation(c.PopulationEffect);
        survivorManager.CheckSurvivorsHealth();
        UpdateAuthority();
        */

    }

    protected int TotalWeight(GameObject[] gos)
    {
        int total = 0;
        for (int z = 0; z < gos.Length; z++)
        {
            Event t = gos[z].GetComponent<Event>();
            total += t.Weight;
        }
        return total;
    }

    protected GameObject RandomEvent(GameObject[] gos)
    {
        int total = TotalWeight(gos);
        int sum = 0;
        GameObject chosenEvent = null;

        int probability = (int)Random.Range(0, total);

        for (int k = 0; k < gos.Length; k++)
        {
            //Debug.Log(gos[k].GetComponent<Event>().text + " Probabilité de " + (float)gos[k].GetComponent<Event>().weight / (float)total);

            sum += gos[k].GetComponent<Event>().Weight;
            if (sum > probability)
            {
                chosenEvent = gos[k];
                break;
            }
        }

        return chosenEvent;
    }

    public void ShowSurvivors()
    {
        if (newPanelSurvivor == null)
        {
            newPanelSurvivor = Instantiate(survivorsPanel);
            newPanelSurvivor.transform.SetParent(HUD.transform, false);
        }
        else
        {
            Destroy(newPanelSurvivor);
        }
    }

    public void ShowEventPanel()
    {
        if (!eventIsActive)
        {
            eventPanel.SetActive(true);
            eventIsActive = true;
        }
        else
        {
            eventPanel.SetActive(false);
            eventIsActive = false;
        }
    }

    public void CloseEventPanel()
    {
        eventPanel.SetActive(false);
        eventIsActive = false;
    }

}
