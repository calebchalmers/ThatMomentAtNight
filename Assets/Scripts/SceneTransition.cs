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
        float from = intro ? -80f : 0f;
        float to = intro ? 0f : -80f;
        float duration = intro ? introFadeDuration : outroFadeDuration;
        StartCoroutine(AnimateFadeAudio(from, to, duration));
    }

    private IEnumerator AnimateFadeAudio(float from, float to, float duration)
    {
        float time = 0f;
        while (time <= duration)
        {
            time = Mathf.Min(time + Time.deltaTime, duration);
            float volume = Mathf.Lerp(from, to, time / duration);
            audioMixer.SetFloat("Master Volume", volume);
            yield return null;
        }
    }

    public static SceneTransition Find()
    {
        return GameObject.FindGameObjectWithTag("SceneTransition")?.GetComponent<SceneTransition>();
    }
}
