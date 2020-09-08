using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingController : MonoBehaviour
{
    public CutsceneSequence cutscene;
    public TextAsset ending1;
    public TextAsset ending2;
    public TextAsset ending3;
    public TextAsset ending4;

    public List<Animator> CreditsSet1;
    public List<Animator> CreditsSet2;
    public List<Animator> CreditsSet3;
    public List<Animator> CreditsSet4;

    public void SetEnding(int i)
    {
        if (i == 1)
            cutscene.dialogueTextAsset = ending1;
        if (i == 2)
            cutscene.dialogueTextAsset = ending2;
        if (i == 3)
            cutscene.dialogueTextAsset = ending3;
        if (i == 4)
            cutscene.dialogueTextAsset = ending4;
    }

    public void ShowCreditGroup(int i)
    {
        if (i == 1)
            StartCoroutine(DisplayCredits(CreditsSet1));
        if (i == 2)
            StartCoroutine(DisplayCredits(CreditsSet2));
        if (i == 3)
            StartCoroutine(DisplayCredits(CreditsSet3));
        if (i == 4)
            StartCoroutine(DisplayCredits(CreditsSet4));
    }

    public void HideCreditGroup(int i)
    {
        if (i == 1)
            StartCoroutine(HideCredits(CreditsSet1));
        if (i == 2)
            StartCoroutine(HideCredits(CreditsSet2));
        if (i == 3)
            StartCoroutine(HideCredits(CreditsSet3));
        if (i == 4)
            StartCoroutine(HideCredits(CreditsSet4));
    }

    private IEnumerator DisplayCredits(List<Animator> Credits)
    {
        foreach(Animator anim in Credits)
        {
            Debug.Log(anim);
            anim.SetTrigger("Appear");
            yield return new WaitForSeconds(0.75f);
        }
    }

    private IEnumerator HideCredits(List<Animator> Credits)
    {
        foreach (Animator anim in Credits)
        {
            yield return new WaitForSeconds(0.3f);
            anim.SetTrigger("Disappear");
        }
    }
}
