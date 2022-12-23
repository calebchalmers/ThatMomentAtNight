using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class VolumeControl
{
    private AudioMixer mixer;

    private static float defaultSfx = 0f;
    private static float defaultMusic = 0f;
    private static float defaultVoice = 0f;
    private static bool initialized = false;

    public VolumeControl(AudioMixer mixer)
    {
        this.mixer = mixer;

        if (!initialized)
        {
            mixer.GetFloat("SFX Volume", out defaultSfx);
            mixer.GetFloat("Music Volume", out defaultMusic);
            mixer.GetFloat("Voice Volume", out defaultVoice);
            initialized = true;
        }
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