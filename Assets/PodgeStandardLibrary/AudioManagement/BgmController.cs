using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmController : MonoBehaviour {

    public AudioSource audioBgmSrcMain;
    public AudioSource audioBgmSrcSecondary;
    public AudioSource audioTransitionSrc;

    public AudioClip currentAudio;

    public void SwitchTrack(AudioClip track, float transitionOverlap = 0.0f, float transitionTime = 1.0f)
    {
        currentAudio = track;

        StartCoroutine(CrossFadeTrack(track, transitionOverlap, transitionTime));
    }

    public void SwitchTrackWithTransition(AudioClip transition, AudioClip track, float transitionOverlap = 0.0f, float transitionTime = 1.0f)
    {
        currentAudio = track;

        StartCoroutine(TransitionTrack(transition,  track, transitionOverlap, transitionTime));
    }

    private IEnumerator CrossFadeTrack(AudioClip track, float trackOffset, float transitionOverlap = 0.0f, float transitionTime = 1.0f)
    {
        audioBgmSrcSecondary.clip = track;
        audioBgmSrcSecondary.volume = 0.0f;

        float timer = 0.0f;
        float totalTransitionTime = transitionTime + (1 - transitionTime) * transitionTime;


        while (timer < totalTransitionTime)
        {
            timer += Time.deltaTime;
            audioBgmSrcMain.volume = Mathf.Lerp(1.0f, 0.0f, timer / transitionTime);
            audioBgmSrcSecondary.volume = Mathf.Lerp(0.0f, 1.0f, (timer-(totalTransitionTime - transitionTime)) / transitionTime);

            if (!audioBgmSrcSecondary.isPlaying && audioBgmSrcSecondary.volume > 0)
                audioBgmSrcSecondary.Play();

            if (audioBgmSrcMain.volume == 0)
                audioBgmSrcMain.Stop();

            yield return new WaitForEndOfFrame();
        }

        AudioSource audioSrcTemp = audioBgmSrcMain;
        audioBgmSrcMain = audioBgmSrcSecondary;
        audioBgmSrcSecondary = audioSrcTemp;

        yield return null;
    }


    const float transitionSmoothingDuration = 0.3f;
    private IEnumerator TransitionTrack(AudioClip transition, AudioClip track, float transitionOverlap = 0.9f, float transitionTime = 1.0f)
    {
        float timer = 0.0f;
        float totalTransitionTime = transitionTime + (1 - transitionTime) * transitionTime;

        while (timer < totalTransitionTime)
        {
            timer += Time.deltaTime;
            audioBgmSrcMain.volume = Mathf.Lerp(1.0f, 0.0f, timer / transitionTime);

            if (!audioTransitionSrc.isPlaying && timer > (totalTransitionTime - transitionTime))
            {
                audioTransitionSrc.volume = 1.0f;
                audioTransitionSrc.clip = transition;
                audioTransitionSrc.Play();
            }

            if (audioBgmSrcMain.volume == 0)
                audioBgmSrcMain.Stop();

            yield return new WaitForEndOfFrame();
        }

        while(audioTransitionSrc.time < audioTransitionSrc.clip.length - transitionSmoothingDuration)
        {
            yield return new WaitForEndOfFrame();
        }


        audioBgmSrcSecondary.volume = 0.0f;
        audioBgmSrcSecondary.clip = track;
        audioBgmSrcSecondary.Play();
        timer = 0.0f;
        while (timer < transitionSmoothingDuration)
        {
            timer += Time.deltaTime;
            audioTransitionSrc.volume = Mathf.Lerp(1.0f, 0.0f, timer / transitionSmoothingDuration);
            audioBgmSrcSecondary.volume = Mathf.Lerp(0.0f, 1.0f, timer / transitionSmoothingDuration);

            yield return new WaitForEndOfFrame();
        }


        AudioSource audioSrcTemp = audioBgmSrcMain;
        audioBgmSrcMain = audioBgmSrcSecondary;
        audioBgmSrcSecondary = audioSrcTemp;

        yield return null;
    }
    
    private IEnumerator FadeTowards (float targetVolume, float duration=1.0f) {
        float timer = 0.0f;

        float startingVolume = audioBgmSrcMain.volume;
        while(timer < duration)
        {
            timer += Time.deltaTime;
            audioBgmSrcMain.volume = Mathf.Lerp(startingVolume, targetVolume, timer / duration);
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
}

[Serializable]
public struct AudioTrack
{
    public string name;
    public AudioClip track;

    public AudioTrack(string name, AudioClip track)
    {
        this.name = name;
        this.track = track;
    }
}