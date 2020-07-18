using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReisenSceneManager : MonoBehaviour
{
    public Bounds CameraBounds;
    public List<SceneSwitchTrigger> sceneEntrances;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray - Color.black * 0.6f;
        Gizmos.DrawCube(CameraBounds.center, CameraBounds.extents * 2);  
    }
}
