using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

	public Text nameText;
	public Text dialogueText;
    public Text titleText;
    public Button continueButton;
    public GameObject eventHUD;
    public AudioSource TypingSound;

    protected static DialogueManager instance;
    protected bool isRunning = false;
    private Queue<string> sentences;

    public static DialogueManager GetInstance()
    {
        if (instance == null)
        {
            Debug.Log("No instance of " + typeof(DialogueManager));
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
    void Start () {
        sentences = new Queue<string>();
	}
    private void Update()
    {
    }

    public void StartDialogue (Dialogue dialogue)
	{
        //DialogPanel.GetComponent<Animator>().SetBool("IsOpen", true);
        Debug.Log("Starting conversation with" + dialogue.name);

		nameText.text = dialogue.name;
        titleText.text = dialogue.name;

		//sentences.Clear();

		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}

		DisplayNextSentence();
	}

	public void DisplayNextSentence ()
	{
		if (sentences.Count > 1)
		{
            eventHUD.SetActive(false);
            //timerIsActive = true;
            continueButton.gameObject.SetActive(true);
        }

        if (sentences.Count == 1)
        {
            continueButton.gameObject.SetActive(false);
            eventHUD.SetActive(true);
        }
        
        
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        if (!isRunning)
        {
            string sentence = sentences.Dequeue();
            StartCoroutine(TypeSentence(sentence));
        }
    }

	IEnumerator TypeSentence (string sentence)
	{
        isRunning = true;
        dialogueText.text = "";
        TypingSound.Play();
        if (sentence != null)
        {
            foreach (char letter in sentence.ToCharArray())
            {
                dialogueText.text += letter;
                yield return null;
            }
        }
        TypingSound.Stop();
        isRunning = false;
        /*
        if (isRunning == false)
        {
            isRunning = true;
            dialogueText.text = "";
            if (waiting.Count == 0)
            {
                foreach (char letter in sentence.ToCharArray())
                {
                    dialogueText.text += letter;
                    yield return null;
                }
            }
            else
            {
                waiting.Enqueue(sentence);
                string s = waiting.Dequeue();
                foreach (char letter in s.ToCharArray())
                {
                    dialogueText.text += letter;
                    yield return null;
                }
                
            }
            isRunning = false;
        }
        else
        {
            waiting.Enqueue(sentence);
        }*/

    }

	void EndDialogue()
	{

    }

    public void ValidateButton ()
    {
        //continueButton.gameObject.SetActive(true);
        //eventHUD.SetActive(false);
    }

}
