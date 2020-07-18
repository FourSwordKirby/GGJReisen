using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YinYangOrbAnimator : MonoBehaviour
{
    public Color ReimuColor;
    public Color YukariColor;

    public Animator selfAnimator;
    public SpriteRenderer orbSprite;
    public Light orbLight;

    internal void Enter()
    {
        orbSprite.enabled = true;
        orbLight.enabled = true;
        selfAnimator.SetTrigger("Enter");
    }


    internal void Exit()
    {
        selfAnimator.SetTrigger("Exit");
    }

    public IEnumerator SwitchYukari()
    {
        float lightTransitionTime = 1.0f;
        float timer = 0.0f;
        while(timer < lightTransitionTime)
        {
            Color targetColor = Color.Lerp(ReimuColor, YukariColor, timer / lightTransitionTime);

            orbSprite.color = targetColor;
            orbLight.color = targetColor;

            timer += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }
}
