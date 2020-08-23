using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

public class AudioMaster : MonoBehaviour
{
    public AudioMixer masterMixer;
    //public List<AudioMixerSnapshot> snapshots;

    public BgmController bgmController;
    public List<AudioTrack> bgmAudioMixes;

    public SfxController sfxController;
    public List<AudioTrack> soundEffects;


    public AudioClip battleStart;

    [Range(0f, 1f)]
    public float masterVolume;

    [Range(0f, 1f)]
    public float bgmVolume;

    [Range(0f, 1f)]
    public float sfxVolume;

    [Range(0f, 1f)]
    public float voiceVolume;
    public static AudioMaster instance;

    public void Awake()
    {
        if (AudioMaster.instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (this != instance)
            Destroy(this.gameObject);

    }

    public void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    bgmController.SwitchTrack(bgmAudioMixes[0].track);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    bgmController.SwitchTrack(bgmAudioMixes[1].track);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    bgmController.SwitchTrack(bgmAudioMixes[2].track);
        //}

        //if(Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    bgmController.SwitchTrackWithTransition(battleStart, bgmAudioMixes[0].track);
        //}
        
        masterMixer.SetFloat("MasterVolume", Mathf.Log10(Mathf.Lerp(0.0001f, 1.0f, masterVolume)) * 20);
        masterMixer.SetFloat("BgmVolume", Mathf.Log10(Mathf.Lerp(0.0001f, 1.0f, bgmVolume)) * 20);
        masterMixer.SetFloat("SfxVolume", Mathf.Log10(Mathf.Lerp(0.0001f, 1.0f, sfxVolume)) * 20);
        masterMixer.SetFloat("VoiceVolume", Mathf.Log10(Mathf.Lerp(0.0001f, 1.0f, voiceVolume)) * 20);
    }

    internal void PlayTrack(string trackName)
    {
        AudioTrack track = bgmAudioMixes.Find(x => x.name == trackName);
        if (track.track == null)
            Debug.Log("couldn't find bgm track: " + trackName);

        if (bgmController.currentAudio != track.track)
            bgmController.SwitchTrack(track.track);
    }

    public void PlayPrologueTrack()
    {
        AudioTrack prologueTrack = bgmAudioMixes.Find(x => x.name == "Prologue");

        if (bgmController.currentAudio != prologueTrack.track)
            bgmController.SwitchTrack(prologueTrack.track);
    }

    internal void PlayVillageTrack()
    {
        AudioTrack townTrack = bgmAudioMixes.Find(x => x.name == "Town");

        if (bgmController.currentAudio != townTrack.track)
            bgmController.SwitchTrack(townTrack.track);
    }

    internal void PlaySpookyTrack()
    {
        Debug.Log("this should be a spooky track... if we had one lol");

        //AudioTrack track = bgmAudioMixes.Find(x => x.name == "Spooky");

        //if (bgmController.currentAudio != track.track)
        //    bgmController.SwitchTrack(track.track);
    }

    internal void StopSpookyTrack()
    {
        Debug.Log("this should stop a spooky track... if we had one lol");
        //bgmController.StopTrack();
    }

    internal void PlaySfx(string sfxName)
    {
        AudioTrack sfx = soundEffects.Find(x => x.name == sfxName);
        if (sfx.track == null)
            Debug.Log("couldn't find sfx: " + sfxName);
        sfxController.playSfx(sfx.track, 1.0f);
    }

    internal void PlayMenuSelectSfx()
    {
        AudioTrack sfx = soundEffects.Find(x => x.name == "MenuSelect");
        sfxController.playSfx(sfx.track, 1.0f);
    }


    internal void PlayConfirmSfx()
    {
        AudioTrack sfx = soundEffects.Find(x => x.name == "Confirm");
        sfxController.playSfx(sfx.track, 1.0f);
    }


    internal void PlayCancelSfx()
    {
        AudioTrack sfx = soundEffects.Find(x => x.name == "Cancel");
        sfxController.playSfx(sfx.track, 1.0f);
    }

    internal void PlayTransformSfx()
    {
        AudioTrack sfx = soundEffects.Find(x => x.name == "Transform");
        sfxController.playSfx(sfx.track, 1.0f);
    }

    internal void PlayItemGetSfx()
    {
        AudioTrack sfx = soundEffects.Find(x => x.name == "ItemGet");
        sfxController.playSfx(sfx.track, 1.0f);
    }

    internal void PlayDialogueAdvanceSfx()
    {
        AudioTrack sfx = soundEffects.Find(x => x.name == "DialogueAdvance");
        sfxController.playSfx(sfx.track, 1.0f);
    }


    internal void PlayJumpSfx()
    {
        AudioTrack sfx = soundEffects.Find(x => x.name == "Jump");
        sfxController.playSfx(sfx.track, 0.6f);
    }

    internal void PlayTeleportEnterSfx()
    {
        AudioTrack sfx = soundEffects.Find(x => x.name == "TeleportEnter");
        sfxController.playSfx(sfx.track, 0.6f);
    }
}

