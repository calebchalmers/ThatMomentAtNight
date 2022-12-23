using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class VolumeControl
{
    private AudioMixer mixer;
    private float defaultSfx = 0f;
    private float defaultMusic = 0f;
    private float defaultVoice = 0f;

    public VolumeControl(AudioMixer mixer)
    {
        this.mixer = mixer;
        mixer.GetFloat("SFX Volume", out defaultSfx);
        mixer.GetFloat("Music Volume", out defaultMusic);
        mixer.GetFloat("Voice Volume", out defaultVoice);
    }

    public void ChangeSfx(float volume)
    {
        mixer.SetFloat("SFX Volume", Mathf.Lerp(-80f, defaultSfx, volume));
    }

    public void ChangeMusic(float volume)
    {
        mixer.SetFloat("Music Volume", Mathf.Lerp(-80f, defaultMusic, volume));
    }

    public void ChangeVoice(float volume)
    {
        mixer.SetFloat("Voice Volume", Mathf.Lerp(-80f, defaultVoice, volume));
    }
}