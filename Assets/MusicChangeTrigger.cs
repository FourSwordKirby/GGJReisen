using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChangeTrigger : MonoBehaviour
{
    public string trackName;
    public void OnTriggerEnter(Collider other)
    {
        AudioMaster.instance.PlayTrack(trackName);
    }
}
