using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CutsceneUI : MonoBehaviour
{
    public static CutsceneUI instance;
    public TextMeshProUGUI text;
    public Image CgImage;
    internal bool dialogueLineFinished;

    public Color cgColor = Color.white;
    public Color textColor;

    public void Awake()
    {
        if (CutsceneUI.instance == null)
            instance = this;
        else if (this != instance)
            Destroy(this.gameObject);

        text.text = "";
    }

    internal void FadeCg()
    {
        StartCoroutine(animateFadeCg());
    }

    private IEnumerator animateFadeCg()
    {
        float FADE_TIME = 0.2f;
        float fadeTimer = 0.0f;

        // fading out;
        if (CgImage.color.a == 1.0)
        {
            while (fadeTimer < FADE_TIME)
            {
                fadeTimer += Time.deltaTime;
                CgImage.color = Color.Lerp(Color.white, Color.clear, fadeTimer / FADE_TIME);
                yield return new WaitForEndOfFrame();
            }
        }
    }

    public void DisplayCg(Sprite CgSprite)
    {
        StartCoroutine(animateCg(CgSprite));
    }

    private IEnumerator animateCg(Sprite CgSprite)
    {
        float FADE_TIME = 0.2f;
        float fadeTimer = 0.0f;

        // fading out;
        if(CgImage.color.a == 1.0)
        {
            while (fadeTimer < FADE_TIME)
            {
                fadeTimer += Time.deltaTime;
                CgImage.color = Color.Lerp(Color.white, Color.clear, fadeTimer / FADE_TIME);
                yield return new WaitForEndOfFrame();
            }
        }

        CgImage.sprite = CgSprite;

        // fading in;
        fadeTimer = 0.0f;
        while (fadeTimer < FADE_TIME)
        {
            fadeTimer += Time.deltaTime;
            CgImage.color = Color.Lerp(CgImage.color, cgColor, fadeTimer / FADE_TIME);
            yield return new WaitForEndOfFrame();
        }
    }

    internal void DisplayText(SpeakingLineContent content)
    {
        StartCoroutine(animateText(content));
    }

    private IEnumerator animateText(SpeakingLineContent content)
    {
        dialogueLineFinished = false;
        float FADE_TIME = 0.2f;
        float fadeTimer = 0.0f;

        // fading out;
        while(fadeTimer < FADE_TIME)
        {
            fadeTimer += Time.deltaTime;
            text.color = Color.Lerp(textColor, Color.clear, fadeTimer / FADE_TIME);
            yield return new WaitForEndOfFrame();
        }

        text.text = content.lineText;

        // fading in;
        fadeTimer = 0.0f;
        while (fadeTimer < FADE_TIME)
        {
            fadeTimer += Time.deltaTime;
            text.color = Color.Lerp(Color.clear, textColor, fadeTimer / FADE_TIME);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(0.2f);
        while (!Controls.confirmInputDown())
        {
            yield return null;
        }

        dialogueLineFinished = true;
    }
}
