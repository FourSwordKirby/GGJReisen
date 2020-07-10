using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SavePanelUI : MonoBehaviour
{
    public string fileName;

    public TextMeshProUGUI locationText;
    public TextMeshProUGUI shardCountText;
    public TextMeshProUGUI saveTimeText;

    public Image panel;

    public Color focusColor;
    public Color blurColor;

    public void InitializeSavePanel(GGJReisenSave save)
    {
        locationText.text = save.gameProgress.savePoint.locationName;
        shardCountText.text = "Shards: " + save.gameProgress.Player.ShardsAcquired.Count.ToString();
        saveTimeText.text = save.saveTime;
    }

    internal void InitializeSavePanel()
    {
        locationText.text = "---";
        shardCountText.text = "Shards: -";
        saveTimeText.text = "--/--/-- --:--:--";
    }

    internal void Focus()
    {
        panel.color = focusColor;
    }

    internal void Blur()
    {
        panel.color = blurColor;
    }
}
