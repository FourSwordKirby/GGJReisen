using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementAnimator : MonoBehaviour
{
    public CharacterMovement movement;
    public Animator animator;
    public float bobModifier = 1.0f;

    public ParticleSystem dashParticleSystem;
    public float emitSpeed;
    public ParticleSystem poofParticleSystem;

    bool forcedMove;

    // Update is called once per frame
    void Update()
    {
        if (forcedMove)
            return;

        Vector3 velocityVector = movement.selfBody.velocity;

        float speedModifier = velocityVector.magnitude / movement.walkVelocity * bobModifier;
        float bobbingModifier = speedModifier * (movement.isGrounded ? 1 : 0);
        float swayingModifier = Mathf.Abs(velocityVector.x) / Mathf.Max(velocityVector.magnitude, 1);

        animator.SetFloat("SpeedModifier", speedModifier);
        animator.SetFloat("BobbingModifier", bobbingModifier);
        animator.SetBool("Sway", movement.isGrounded && Mathf.Abs(velocityVector.x) / velocityVector.magnitude > 0.4f && velocityVector.sqrMagnitude > 0.1f);
        
        animator.SetFloat("xDirection", velocityVector.x);

        float zAngle = Mathf.Atan2(velocityVector.z, velocityVector.x) * Mathf.Rad2Deg;

        ParticleSystem.ShapeModule dashPartShape = dashParticleSystem.shape;
        dashPartShape.rotation = Vector3.right * 90 + Vector3.forward * (zAngle + 90);

        ParticleSystem.EmissionModule dashPartEmission = dashParticleSystem.emission;
        dashPartEmission.rateOverTimeMultiplier = bobbingModifier * emitSpeed;

        if (Controls.jumpInputDown() && movement.isGrounded)
        {
            poofParticleSystem.Play();
        }
    }

    //bad hack
    public IEnumerator turnTowards(Vector3 dir)
    {
        animator.SetFloat("SpeedModifier", 1.0f);
        animator.SetFloat("xDirection", dir.x);
        forcedMove = true;
        float timer = 0.5f;

        while(timer > 0)
        {
            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        forcedMove = false;
        yield return null;
    }
}
