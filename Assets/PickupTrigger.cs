using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupTrigger : MonoBehaviour
{
    public GenericPickup pickup;

    private void OnTriggerEnter(Collider other)
    {
        pickup.OnPickup();
    }
}
