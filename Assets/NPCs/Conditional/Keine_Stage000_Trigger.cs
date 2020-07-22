
using System;
using UnityEngine;

public class Keine_Stage000_Trigger : DestroyIfTrue
{
    public Transform CameraLoc1; // Reisen-focus
    public Transform CameraLoc2; // Reisen-focus but turned toward Save Point
    public Transform CameraLoc3; // Save Point Focus

    public void Awake()
    {
        CameraLoc1 = transform.Find(nameof(CameraLoc1) ?? throw new Exception($"{nameof(Keine_Stage000_Trigger)} cannot find {nameof(CameraLoc1)}"));
        CameraLoc2 = transform.Find(nameof(CameraLoc2) ?? throw new Exception($"{nameof(Keine_Stage000_Trigger)} cannot find {nameof(CameraLoc2)}"));
        CameraLoc3 = transform.Find(nameof(CameraLoc3) ?? throw new Exception($"{nameof(Keine_Stage000_Trigger)} cannot find {nameof(CameraLoc3)}"));
    }

    public override bool CheckCondition()
    {
        return GameProgress?.Keine?.Stage > 0;
    }
}