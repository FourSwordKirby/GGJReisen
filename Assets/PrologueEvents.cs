using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrologueEvents : MonoBehaviour
{
    public void DisplayPrologueBackground(string background)
    {
        Debug.Log(background);
    }

    public void LoadVillageEntrance()
    {
        StartCoroutine(SceneSwitchSequence());
    }

    public IEnumerator SceneSwitchSequence()
    {
        SceneManager.sceneLoaded += TransitionManager.instance.FadeInScene;
        yield return TransitionManager.instance.screenFader.FadeOut();
        SceneManager.LoadScene("VillageEntrance");
        ReisenGameManager.isNewGame = true;
        yield return null;
    }
}
