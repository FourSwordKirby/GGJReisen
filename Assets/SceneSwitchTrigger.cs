using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSwitchTrigger : MonoBehaviour
{
    public string targetScene;
    public int sceneEntranceIndex;

    public bool triggerActive = true;
    public Transform spawnArea;
    public Transform spawnFinalPosition;

    private void OnTriggerEnter(Collider col)
    {
        if(triggerActive)
            ReisenGameManager.instance.SceneExit(targetScene, sceneEntranceIndex, this);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(this.transform.position, this.transform.localScale);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(spawnArea.position, 0.5f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(spawnFinalPosition.position, 0.5f);
    }
}
