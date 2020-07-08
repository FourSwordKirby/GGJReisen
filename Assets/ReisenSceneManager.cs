using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReisenSceneManager : MonoBehaviour
{
    public static ReisenSceneManager instance;

    public void Awake()
    {
        if (ReisenSceneManager.instance == null)
            instance = this;
        else if (this != instance)
            Destroy(this.gameObject);
    }

    public List<SceneSwitchTrigger> sceneEntrances;
}
