using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Enum_GameOver
{
    playing,
    lowAuthority,
    highAuthority,
    death
}
public enum Enum_DisplayStatus
{
    hidden,
    background,
    day,
    fadeOut,
    fadeIn
}
public class DayAnnouncer : MonoBehaviour {

    private Village village;
    private SurvivorManager survivorManager;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private Button[] buttons;
    private Text[] TextDisplay;
    private Text dayDisplay;
    private Text scoreDisplay;
    private Text gameOverDisplay;
    private Text gameOverCause;
    private Image background;
    public float timeBeforeDisplay = 1.5f;
    public float displayDuration = 3.0f;
    public float fadeInDuration = 0.5f;
    public float fadeOutDuration = 4.0f;
    public Enum_GameOver gameStatus = Enum_GameOver.playing;

    public string lowAuthorityString = "Vous avez été exilé par la colonie";
    public string highAuthorityString = "Une rebellion a mis fin à votre règne de terreur";
    public string deathString = "La colonisation a échoué";

    public Enum_DisplayStatus displayStatus = Enum_DisplayStatus.hidden;
	// Use this for initialization
	void Start () {
        village = Village.GetInstance();
        survivorManager = SurvivorManager.GetInstance();
        TextDisplay = GetComponentsInChildren<Text>();
        dayDisplay = TextDisplay[0];
        scoreDisplay = TextDisplay[1];
        gameOverDisplay = TextDisplay[2];
        gameOverCause = TextDisplay[3];
        background = GetComponentInChildren<Image>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponent<Canvas>();
        dayDisplay.enabled = false;
        gameOverCause.enabled = false;
        gameOverDisplay.enabled = false;
        buttons = GetComponentsInChildren<Button>();
        foreach (Button b in buttons)
        {
            b.GetComponentInChildren<Text>().enabled = false;
            b.enabled = false;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        switch (gameStatus)
        {
            case Enum_GameOver.lowAuthority:
                gameOverCause.text = lowAuthorityString;
                break;
            case Enum_GameOver.highAuthority:
                gameOverCause.text = highAuthorityString;
                break;
            case Enum_GameOver.death:
                gameOverCause.text = deathString;
                break;
            default:
                break;
        }
        if(gameStatus != Enum_GameOver.playing)
        {
            StopAllCoroutines();
            GameOver();
        }
    }
    void LateUpdate () {
        UpdateText();
	}

    private IEnumerator Background()
    {
        if(canvas == null)
        {
            Debug.Log("undefined");
        }
        canvas.sortingOrder = 3;
        canvasGroup.alpha = 1.0f;
        
        DialogueManager.GetInstance().nameText.text = "";
        DialogueManager.GetInstance().dialogueText.text = "";
        DialogueManager.GetInstance().eventHUD.SetActive(false);

        /*
         * REFACT
         *//*
        textWindowManager.GetInstance().NewWindow();
        textWindowManager.GetInstance().StartWriting();
        textWindowManager.GetInstance().eventHUD.SetActive(false);*/

        yield return new WaitForSecondsRealtime(timeBeforeDisplay);
        StartCoroutine("Day");
        
    }

    private IEnumerator Day()
    {
        dayDisplay.enabled = true;
        scoreDisplay.enabled = true;
        yield return new WaitForSecondsRealtime(displayDuration);
        
        if (gameStatus == Enum_GameOver.playing)
        {
            if (village.CurrentChoice != null)
                StartCoroutine(village.VillageCoroutine());
            else
                village.ChooseEvent();

            StartCoroutine("FadeOut");
        }
        else
        {
            GameOver();
        }
        
        
    }
    
    private IEnumerator FadeOut()
    {
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= 1 / fadeOutDuration * Time.unscaledDeltaTime;
            yield return null;
        }
        if (canvasGroup.alpha == 0.0f)
        {
            dayDisplay.enabled = false;
            scoreDisplay.enabled = false;
            canvas.sortingOrder = -1;
        }
    }

    private IEnumerator FadeIn()
    {
        canvas.sortingOrder = 3;
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += 1 / fadeInDuration * Time.unscaledDeltaTime;
            yield return null;
        }
        StartCoroutine("Background");
    }

    public void UpdateText()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder("Jour ").Append(village.DaysSurvived);
        dayDisplay.text = sb.ToString();
        sb = new System.Text.StringBuilder("Score : ").Append(village.Score);
        scoreDisplay.text = sb.ToString();
    }

    public IEnumerator DisplayDay()
    {
            yield return StartCoroutine("Background");
    }

    public void GameOver()
    {
        foreach(Button b in buttons)
        {
            b.enabled = true;
            b.GetComponentInChildren<Text>().enabled = true;
        }
        canvas.sortingOrder = 3;
        canvasGroup.alpha = 1.0f;
        System.Text.StringBuilder sb = new System.Text.StringBuilder("Score : ").Append(village.Score);
        scoreDisplay.text = sb.ToString();
        scoreDisplay.enabled = true;
        dayDisplay.enabled = false;
        gameOverDisplay.enabled = true;
        gameOverCause.enabled = true;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    public void Reload()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}
