using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSwitchTrigger : MonoBehaviour
{
    public string targetScene;
    public int sceneEntranceIndex;
    public Transform spawnArea;

    private void OnTriggerEnter(Collider col)
    {
        ReisenGameManager.instance.SceneExit(targetScene, sceneEntranceIndex);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(this.transform.position, this.transform.localScale);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(spawnArea.position, 0.5f);
    }
}
