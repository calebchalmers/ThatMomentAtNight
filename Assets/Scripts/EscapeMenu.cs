using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EscapeMenu : MonoBehaviour
{
    public GameObject menu;
    public AudioMixer mixer;
    public Toggle toggleVoice;
    public Toggle toggleSfx;
    public Toggle toggleMusic;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menu.SetActive(!menu.activeSelf);
        }
    }

    float Volume(bool on)
    {
        return on ? 0.0f : -80.0f;
    }

    public void OnToggleVoice()
    {
        mixer.SetFloat("Voice Volume", Volume(toggleVoice.isOn));
    }

    public void OnToggleSfx()
    {
        mixer.SetFloat("SFX Volume", Volume(toggleSfx.isOn));
    }

    public void OnToggleMusic()
    {
        mixer.SetFloat("Music Volume", Volume(toggleMusic.isOn));
    }
}