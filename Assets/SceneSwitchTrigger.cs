using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSwitchTrigger : MonoBehaviour
{
    public string targetScene;
    public int sceneEntranceIndex;

    private void OnTriggerEnter(Collider col)
    {
        ReisenGameManager.instance.SceneExit(targetScene, sceneEntranceIndex);
    }
}
