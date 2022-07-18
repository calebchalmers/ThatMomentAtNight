using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject settingsPanel;

    public Button continueButton;
    public GameObject quitButton;
    public Slider volumeSlider;

    public AudioMixer mixer;

    private SceneTransition sceneTransition;

    void Start()
    {
        sceneTransition = SceneTransition.Find();

#if UNITY_EDITOR || UNITY_STANDALONE
        quitButton.SetActive(true);
#endif
    }

    public void OnPressedNewGame()
    {
        sceneTransition.NextScene();
    }

    public void OnPressedContinue()
    {

    }

    public void OnPressedOpenSettings()
    {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void OnPressedCloseSettings()
    {
        mainPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void OnPressedQuit()
    {
        Application.Quit();
    }

    public void OnVolumeChanged()
    {
        mixer.SetFloat("Volume", Mathf.Lerp(-80f, 0f, volumeSlider.value));
    }
}
