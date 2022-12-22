using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public int nextScene = 1;
    public bool relative = true;
    public bool saveProgress = false;

    private Animator animator;
    private int sceneToLoad = -1;

    void Start()
    {
        animator = GetComponent<Animator>();

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

    private int GetCurrentScene()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public void NextScene()
    {
        int index = nextScene;

        if (relative)
        {
            index += GetCurrentScene();
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
            animator.SetTrigger("next");
        }
    }

    public void LoadNewScene()
    {
        InputLocker.Reset();
        SceneManager.LoadScene(sceneToLoad);
    }

    public static SceneTransition Find()
    {
        return GameObject.FindGameObjectWithTag("SceneTransition")?.GetComponent<SceneTransition>();
    }
}
