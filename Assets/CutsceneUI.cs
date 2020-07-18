using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CutsceneUI : MonoBehaviour
{
    public static CutsceneUI instance;
    public TextMeshProUGUI text;
    internal bool ready;

    public Color textColor;

    public void Awake()
    {
        if (CutsceneUI.instance == null)
            instance = this;
        else if (this != instance)
            Destroy(this.gameObject);

        text.text = "";
    }

    internal void DisplayText(SpeakingLineContent content)
    {
        StartCoroutine(animateText(content));
    }

    private IEnumerator animateText(SpeakingLineContent content)
    {
        ready = false;
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

        ready = true;
    }
}
