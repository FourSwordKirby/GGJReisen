using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AreaNotifcation : MonoBehaviour
{
    [SerializeField]
    private float displayDuration = 3.0f;

    public Animator songAnimator;
    public TextMeshProUGUI AreaTitle;
    public TextMeshProUGUI AreaSubtitle;

    public static AreaNotifcation instance;

    public void Awake()
    {
        if (AreaNotifcation.instance == null)
        {
            instance = this;
        }
        else if (this != instance)
        {
            Destroy(this.gameObject);
            return;
        }
    }

    public void ShowAreaInfo(string areaTitle, string areaSubtitle, float customDisplayDuration = 0.0f)
    {
        if(areaTitle != AreaTitle.text)
            StartCoroutine(ShowAreaInfoequence(areaTitle, areaSubtitle, customDisplayDuration));
    }

    public IEnumerator ShowAreaInfoequence(string areaTitle, string areaSubtitle, float customDisplayDuration = 0.0f)
    {
        songAnimator.SetTrigger("Show");
        AreaTitle.text = areaTitle;
        AreaSubtitle.text = areaSubtitle;

        if (customDisplayDuration == 0)
            customDisplayDuration = displayDuration;
        yield return new WaitForSeconds(customDisplayDuration);
        songAnimator.SetTrigger("Hide");
    }
}
