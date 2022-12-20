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
    public Toggle toggleVoice;
    public Toggle toggleSfx;
    public Toggle toggleMusic;

    void Start()
    {
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