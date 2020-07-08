using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager instance;

    public void Awake()
    {
        if (TransitionManager.instance == null)
            instance = this;
        else if (this != instance)
            Destroy(this.gameObject);
    }

    public void SwitchSceneTransition()
    {
        Debug.Log("switching scene");
    }
}
