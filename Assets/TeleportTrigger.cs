using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
    public TeleportTrigger linkedTrigger;

    public bool triggerActive = true;
    public Transform spawnArea;
    public Transform despawnArea;
    public Transform spawnFinalPosition;

    private void OnTriggerEnter(Collider col)
    {
        if (triggerActive)
        {
            triggerActive = false;
            linkedTrigger.triggerActive = false;
            StartCoroutine(teleportPlayer());
        }
    }

    IEnumerator teleportPlayer()
    {
        GameObject player = RpgPlayer.instance.gameObject;
        CameraMan.instance.TransformToTrack = null;

        yield return player.GetComponent<CharacterMovement>().moveCharacter(despawnArea.position, Vector3.zero, 1.5f, 5.0f);

        player.transform.position = linkedTrigger.spawnArea.position;
        CameraMan.instance.TransformToTrack = player.transform;

        yield return player.GetComponent<CharacterMovement>().moveCharacter(linkedTrigger.spawnFinalPosition.position, Vector3.zero, 1.5f, 5.0f);

        triggerActive = true;
        linkedTrigger.triggerActive = true;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red + Color.yellow - Color.black * 0.5f;
        //Gizmos.DrawCube(this.transform.position, this.transform.localScale);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(spawnArea.position, 0.5f);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(despawnArea.position, 0.5f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(spawnFinalPosition.position, 0.5f);
    }
}
