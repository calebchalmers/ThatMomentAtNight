using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public enum NextBehaviour
    {
        Relative,
        Absolute,
        Saved,
    }

    public int nextScene = 1;
    public NextBehaviour nextBehaviour;
    public bool saveProgress = false;

    [Header("Audio")]
    public AudioMixer audioMixer;
    public float introFadeDuration = 0f;
    public float outroFadeDuration = 0f;

    private Animator animator;
    private int sceneToLoad = -1;

    void Start()
    {
        animator = GetComponent<Animator>();
        FadeAudio(true);

        if (saveProgress)
        {
            PlayerPrefs.SetInt("scene", GetCurrentScene());
            PlayerPrefs.Save();
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Skip Scene"))
        {
            NextScene();
        }
    }

    private int GetSavedScene()
    {
        return PlayerPrefs.GetInt("scene", 0);
    }

    private int GetCurrentScene()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public void NextScene()
    {
        int index = nextScene;

        switch (nextBehaviour)
        {
            case NextBehaviour.Relative:
                index += GetCurrentScene();
                break;

            case NextBehaviour.Saved:
                index = GetSavedScene();
                break;
        }

        GotoScene(index);
    }

    public void RestartScene()
    {
        GotoScene(GetCurrentScene());
    }

    public void GotoScene(int index)
    {
        if (sceneToLoad == -1) // don't override ongoing transition
        {
            sceneToLoad = index;
            FadeAudio(false);
            animator.SetTrigger("next");
        }
    }

    public void LoadNewScene()
    {
        InputLocker.Reset();
        SceneManager.LoadScene(sceneToLoad);
    }

    private void FadeAudio(bool intro)
    {
        float[] weights = { 0.0f, 0.0f };
        float duration = intro ? introFadeDuration : outroFadeDuration;

        if (intro)
        {
            weights[0] = 1.0f;
        }
        else // outro
        {
            weights[1] = 1.0f;
        }

        audioMixer.TransitionToSnapshots(
            new AudioMixerSnapshot[] {
                audioMixer.FindSnapshot("Default"),
                audioMixer.FindSnapshot("Muted")
            },
            weights, duration
        );
    }

    public static SceneTransition Find()
    {
        return GameObject.FindGameObjectWithTag("SceneTransition")?.GetComponent<SceneTransition>();
    }
}
