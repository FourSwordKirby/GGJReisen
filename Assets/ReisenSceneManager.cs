using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReisenSceneManager : MonoBehaviour
{
    public Bounds CameraBounds;
    public List<SceneSwitchTrigger> sceneEntrances;

    public bool renderGizmo;

    private void OnDrawGizmos()
    {
        if(renderGizmo)
        {
            Gizmos.color = Color.gray - Color.black * 0.6f;
            Gizmos.DrawCube(CameraBounds.center, CameraBounds.extents * 2);
        }
    }
}
