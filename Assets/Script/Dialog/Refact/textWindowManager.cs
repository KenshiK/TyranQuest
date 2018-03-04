using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class textWindowManager : MonoBehaviour
{

    public Text nameText;
    public Text dialogueText;
    public Text titleText;
    public Button continueButton;
    public GameObject eventHUD;
    bool click = false;

    protected static textWindowManager instance;
    protected bool isRunning = false;

    public Dictionary<int, List<string>> dictionary = new Dictionary<int, List<string>>();

    public static textWindowManager GetInstance()
    {
        if (instance == null)
        {
            Debug.Log("No instance of " + typeof(textWindowManager));
            return null;
        }
        return instance;
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
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddToDico(int key, string s)
    {
        List<string> list;
        if (dictionary.TryGetValue(key, out list))
        {
            list.Add(s);
        }
        else
        {
            list = new List<string>();
            list.Add(s);
            dictionary.Add(key, list);
        }
    }

    public void AddToDico(int key, string[] strings)
    {
        foreach (string s in strings)
            AddToDico(key, s);
    }

    public void NewWindow(string name, string title)
    {
        Debug.Log("Starting conversation with" + name);

        nameText.text = name;
        titleText.text = name;

        //StartCoroutine(DisplaySentences());
    }

    public void NewWindow()
    {
        NewWindow("", "");
    }

    public void StartWriting()
    {
        StartCoroutine(DisplaySentences());
    }

    public IEnumerator DisplaySentences()
    {
        int j = 0;
        List<string> list;
        Debug.Log("Fouille dico " + dictionary.Count);
        while (dictionary.Count>0)
        {
            if (dictionary.TryGetValue(j, out list))
            {
                Debug.Log("J'ai trouvé une valeur " +j);
                if(list.Count>0)
                {
                    foreach (string s in list)
                    {
                        yield return StartCoroutine(TypeSentence(s));
                    }
                }
                dictionary.Remove(j);
            }
            j++;
        }
        
        dictionary.Clear();
        dictionary = new Dictionary<int, List<string>>();
        Debug.Log("Quit " + dictionary.Count);
    }

    IEnumerator TypeSentence(string sentence)
    {
        click = false;
        dialogueText.text = "";
        if (sentence != null)
        {
            foreach (char letter in sentence.ToCharArray())
            {
                dialogueText.text += letter;
                yield return null;
            }
        }
        if(dictionary.Count>1)
        {
            eventHUD.SetActive(false);
            continueButton.gameObject.SetActive(true);
        }
        if (dictionary.Count == 0 || dictionary.Count == 1)
        {
            continueButton.gameObject.SetActive(false);
            eventHUD.SetActive(true);
        }

    }

    public void Clicked()
    {
        click = true;
    }
}
