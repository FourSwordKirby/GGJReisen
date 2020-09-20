using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaNameChangeTrigger : MonoBehaviour
{
    public string AreaName;
    public void OnTriggerEnter(Collider other)
    {
        AreaNotifcation.instance.ShowAreaInfo(AreaName, "");
    }
}