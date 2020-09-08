using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SongNotification : MonoBehaviour
{
    [SerializeField]
    private float displayDuration;

    public Animator songAnimator;
    public TextMeshProUGUI SongTitle;
    public TextMeshProUGUI SongArtist;

    //private void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.I))
    //    {
    //        StartCoroutine(ShowSongInfo("Placeholder Song Title", "Placeholder Song Artist"));
    //    }
    //}


    public void ShowSongInfo(string songTitle, string songArtist, float customDisplayDuration = 0.0f)
    {
        StartCoroutine(ShowSongInfoSequence(songTitle, songArtist, customDisplayDuration));
    }

    public IEnumerator ShowSongInfoSequence(string songTitle, string songArtist, float customDisplayDuration = 0.0f)
    {
        songAnimator.SetTrigger("Show");
        SongTitle.text = songTitle;
        SongArtist.text = songArtist;

        if (customDisplayDuration == 0)
            customDisplayDuration = displayDuration;
        yield return new WaitForSeconds(customDisplayDuration);
        songAnimator.SetTrigger("Hide");
    }
}
