using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEvents : MonoBehaviour
{
    public YinYangOrbAnimator orb;
    public Animator orbAnimator;
    public GameObject CantLeaveZone;

    public void EnterOrb()
    {
        orb.Enter();
    }

    public void ExitOrb()
    {
        orb.Exit();
    }


    public void SwitchYukari()
    {
        StartCoroutine(orb.SwitchYukari());
    }

    public void SwitchReimu()
    {
        Debug.Log("Reimu");
    }

    public void CantLeave()
    {
        CantLeaveZone.SetActive(true);
    }
}
