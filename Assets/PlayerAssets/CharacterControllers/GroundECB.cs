using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundECB : MonoBehaviour
{
    public CharacterMovement player;

    private void OnTriggerEnter(Collider collision)
    {
        if (player.selfBody.velocity.y <= 0.0f)
            player.isGrounded = true;
    }

    void OnTriggerStay(Collider col)
    {
        if (player.selfBody.velocity.y <= 0.0f)
            player.isGrounded = true;
        else
            player.isGrounded = false;
    }

    void OnTriggerExit(Collider col)
    {
        player.isGrounded = false;
    }
}
