using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public ScreenFader screenFader;

    public static TransitionManager instance;

    public void Awake()
    {
        if (TransitionManager.instance == null)
        {
            instance = this;
        }
        else if (this != instance)
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += FadeInScene;
    }

    public void FadeInScene(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(screenFader.FadeIn());
    }
}
