using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float speed;
    public float jumpPower;

    public Rigidbody selfBody;

    public float walkVelocity;
    public const float maxTurnRotation = 30.0f; //notes for how far the model is allowed to tilt/rotate
    public const float maxTiltRotation = 10.0f;

    public Vector2 OverrideMovementVector = Vector2.zero;

    public bool isGrounded;

    private float fallMultiplier = 2.5f;
    private float lowJumpMultiplier = 2.0f;


    // Update is called once per frame
    void Update()
    {
        Vector2 movementVector = Controls.getDirection();
        if (OverrideMovementVector != Vector2.zero)
        {
            movementVector = OverrideMovementVector;
        }
        selfBody.velocity += (Vector3.right * movementVector.x + Vector3.forward * movementVector.y).normalized;

        Vector3 vel = selfBody.velocity;
        vel.x = Mathf.Min(Mathf.Abs(vel.x), speed) * Mathf.Sign(vel.x);
        vel.z = Mathf.Min(Mathf.Abs(vel.z), speed) * Mathf.Sign(vel.z);
        selfBody.velocity = vel;

        if (Controls.jumpInputDown() && isGrounded)
        {
            isGrounded = false;
            selfBody.velocity += Vector3.up * jumpPower;
        }
    }
}
