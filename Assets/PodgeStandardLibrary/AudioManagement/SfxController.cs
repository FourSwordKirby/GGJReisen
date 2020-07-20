﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxController : MonoBehaviour {

    public AudioSource audioSrc;

    public void playSfx(AudioClip sfx, float volume)
    {
        audioSrc.PlayOneShot(sfx, volume);
    }
}