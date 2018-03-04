using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private Text[] TextDisplay;
    private Text dayDisplay;
    private Text scoreDisplay;
    private Image background;
    public float timeBeforeDisplay = 1.5f;
    public float displayDuration = 3.0f;
    public float fadeInDuration = 0.5f;
    public float fadeOutDuration = 4.0f;
    
    public Enum_DisplayStatus displayStatus = Enum_DisplayStatus.hidden;
    private float displayTimer, fadeTimer;
	// Use this for initialization
	void Start () {
        village = Village.GetInstance();
        TextDisplay = GetComponentsInChildren<Text>();
        dayDisplay = TextDisplay[0];
        scoreDisplay = TextDisplay[1];
        background = GetComponentInChildren<Image>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponent<Canvas>();
        dayDisplay.enabled = false;
	}
	
	// Update is called once per frame
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

        if (village.CurrentChoice != null)
            StartCoroutine(village.VillageCoroutine());
        else
            village.ChooseEvent();

        StartCoroutine("FadeOut");
        
    }
    
    private IEnumerator FadeOut()
    {
        village.ShowSurvivors();
        village.ShowSurvivors();
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

}
