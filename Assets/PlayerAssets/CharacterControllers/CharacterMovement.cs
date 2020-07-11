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

    public bool forcedMove;
    public Vector3 targetPosition = Vector3.zero;
    public Vector3 forwardDirection = Vector3.zero;

    public bool isGrounded;

    // Update is called once per frame
    void Update()
    {
        Vector2 movementVector = Vector2.zero;
        if (!forcedMove)
            movementVector = Controls.getDirection();
        selfBody.velocity += (Vector3.right * movementVector.x + Vector3.forward * movementVector.y).normalized;

        Vector3 vel = selfBody.velocity;
        vel.x = Mathf.Min(Mathf.Abs(vel.x), speed * Mathf.Cos(Mathf.Atan2(Mathf.Abs(vel.z), Mathf.Abs(vel.x)))) * Mathf.Sign(vel.x);
        vel.z = Mathf.Min(Mathf.Abs(vel.z), speed * Mathf.Sin(Mathf.Atan2(Mathf.Abs(vel.z), Mathf.Abs(vel.x)))) * Mathf.Sign(vel.z);
        selfBody.velocity = vel;

        if (Controls.jumpInputDown() && isGrounded)
        {
            isGrounded = false;
            selfBody.velocity += Vector3.up * jumpPower;
        }
    }

    public IEnumerator moveCharacter(Vector3 targetPosition, Vector3 facingDirection)
    {
        forcedMove = true;
        Vector3 displacement = (targetPosition - this.transform.position);
        Vector3 currentDisplacement = displacement;
        while(currentDisplacement.sqrMagnitude > 0.2f)
        {
            currentDisplacement = (targetPosition - this.transform.position);
            selfBody.velocity = (Vector3.right * currentDisplacement.x + Vector3.forward * currentDisplacement.z).normalized * 2.0f;
            yield return new WaitForEndOfFrame();
        }
        //dumb hack for cutscen ease
        yield return GetComponent<CharacterMovementAnimator>().turnTowards(facingDirection);

        selfBody.velocity = Vector3.zero;
        forcedMove = false;
        yield return null;
    }
}
