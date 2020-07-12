using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour {
    public SaveUI saveUI;
    public static PauseUI instance;

    public void Awake()
    {
        if (PauseUI.instance == null)
            instance = this;
        else if (this != instance)
            Destroy(this.gameObject);
    }
}
