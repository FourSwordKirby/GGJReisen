using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RpgPlayer : MonoBehaviour
{
    public static RpgPlayer instance;

    public void Awake()
    {
        if (RpgPlayer.instance == null)
        {
            instance = this;
        }
        else if (this != instance)
            Destroy(this.gameObject);
    }
}
