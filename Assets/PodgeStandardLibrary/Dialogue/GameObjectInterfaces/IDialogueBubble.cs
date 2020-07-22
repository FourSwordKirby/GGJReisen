using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public interface IDialogueBubble
{
    void DeployAt(Vector3 speakerPosition, Vector3 displacementVector, Quaternion rotation);

    void Show();
    void Hide();

    void Focus();
    void Blur();

    void Cleanup();

    void Destroy();
}
