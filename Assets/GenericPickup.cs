using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericPickup : MonoBehaviour
{
    public Animator animator;
    public ItemDesignation itemDesignation;
    public float itemCount = 1;
    public GameObject model;


    public void OnPickup()
    {
        animator.SetTrigger("Pickup");

        ReisenGameManager.instance.gameProgress.Player.AddItem(itemDesignation, (int)itemCount);
    }

    void Cleanup()
    {
        Destroy(this.gameObject);
    }
}
