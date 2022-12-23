using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EscapeMenu : MonoBehaviour
{
    public bool showOnStart = false;
    public GameObject menu;
    public AudioMixer mixer;
    public Slider sliderVoice;
    public Slider sliderSfx;
    public Slider sliderMusic;

    private VolumeControl volumeControl;

    void Start()
    {
        volumeControl = new VolumeControl(mixer);

        float volume_voice = PlayerPrefs.GetFloat("volume_voice", 1f);
        volumeControl.ChangeVoice(volume_voice);
        sliderVoice.value = volume_voice;

        float volume_sfx = PlayerPrefs.GetFloat("volume_sfx", 1f);
        volumeControl.ChangeSfx(volume_sfx);
        sliderSfx.value = volume_sfx;

        float volume_music = PlayerPrefs.GetFloat("volume_music", 1f);
        volumeControl.ChangeMusic(volume_music);
        sliderMusic.value = volume_music;

        if (showOnStart)
        {
            SetShowing(true);
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Menu"))
        {
            SetShowing(!IsShowing());
        }
    }

    float Volume(bool on)
    {
        return on ? 0.0f : -80.0f;
    }

    public void OnVolumeVoice()
    {
        float volume = sliderVoice.value;
        volumeControl.ChangeVoice(volume);
        PlayerPrefs.SetFloat("volume_voice", volume);
    }

    public void OnVolumeSfx()
    {
        float volume = sliderSfx.value;
        volumeControl.ChangeSfx(volume);
        PlayerPrefs.SetFloat("volume_sfx", volume);
    }

    public void OnVolumeMusic()
    {
        float volume = sliderMusic.value;
        volumeControl.ChangeMusic(volume);
        PlayerPrefs.SetFloat("volume_music", volume);
    }

    public void Resume()
    {
        SetShowing(false);
    }

    public void ExitToMenu()
    {
        SceneTransition.Find().GotoScene(1);
    }

    public void SetShowing(bool show)
    {
        menu.SetActive(show);

        if (show)
        {
            InputLocker.Lock("EscapeMenu");
        }
        else
        {
            InputLocker.Unlock("EscapeMenu");
        }
    }

    public bool IsShowing()
    {
        return menu.activeSelf;
    }
}