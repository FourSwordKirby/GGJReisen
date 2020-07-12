using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    public ScreenFader screenFader;

    public static TransitionManager instance;

    public void Awake()
    {
        if (TransitionManager.instance == null)
            instance = this;
        else if (this != instance)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }
}
