using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneEvents : MonoBehaviour
{
    public List<CutsceneCg> CutsceneCgs;

    public void DisplayCg(string CgName)
    {
        Sprite CgSprite = CutsceneCgs.Find(x => x.name == CgName).CgSprite;
        CutsceneUI.instance.DisplayCg(CgSprite);
    }


    public void FadeCg()
    {
        CutsceneUI.instance.FadeCg();
    }

    public void LoadTitle()
    {
        StartCoroutine(ReturnToTitleSequence());
    }

    public void LoadVillageEntrance()
    {
        StartCoroutine(SceneSwitchSequence());
    }

    public void DisplayCredits()
    {
    }

    public void ConfirmEnding()
    {
    }

    public IEnumerator ReturnToTitleSequence()
    {
        yield return new WaitForSeconds(3.0f);
        yield return TransitionManager.instance.screenFader.FadeOut();
        SceneManager.LoadScene("TitleScreen");
        yield return null;
    }

    public IEnumerator SceneSwitchSequence()
    {
        yield return TransitionManager.instance.screenFader.FadeOut();
        SceneManager.LoadScene("VillageEntrance");
        ReisenGameManager.isNewGame = true;
        yield return null;
    }
}

[Serializable]
public struct CutsceneCg
{
    public string name;
    public Sprite CgSprite;
}
