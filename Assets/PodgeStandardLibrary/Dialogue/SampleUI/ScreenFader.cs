﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    public Image screen;
    public bool fading;
    public bool fadeActive { get { return this.screen.color != Color.clear; } set {; }}

    void Awake()
    {
        //this.screen.color = Color.clear;
    }

    public IEnumerator FadeOut(float fadeTime = 1.0f, float lingerTime = 0.3f)
    {
        fading = true;
        float timer = 0.0f;

        Color initialColor = screen.color;

        while (timer < fadeTime + lingerTime)
        {
            timer += Time.deltaTime;
            screen.color = Color.Lerp(initialColor, Color.black, timer / fadeTime);
            yield return new WaitForEndOfFrame();
        }
        screen.color = Color.black;
        fading = false;
        yield return null;
    }

    public IEnumerator FadeIn(float fadeTime = 1.0f, float delayTime = 0.3f)
    {
        fading = true;
        float timer = 0;

        Color initialColor = screen.color;

        while (timer < fadeTime + delayTime)
        {
            timer += Time.deltaTime;
            screen.color = Color.Lerp(initialColor, Color.clear, (timer- delayTime) / fadeTime);

            yield return new WaitForEndOfFrame();
        }
        fading = false;
        yield return null;
    }

    public IEnumerator Dim()
    {
        fading = true;
        float timer = 0.0f;
        while (timer < 2)
        {
            timer += Time.deltaTime;
            if (timer < 2)
            {
                screen.color = Color.Lerp(Color.black - Color.black, Color.black, timer / 4);
                yield return new WaitForSeconds(0.01f);
            }
        }
        fading = false;
        yield return null;
    }

    /// <summary>
    /// This is used to force the screenfader to suddenly be set to some specified color
    /// </summary>
    public void CutToColor(Color color)
    {
        screen.color = color;
        fading = false;
    }
}