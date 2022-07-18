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

    private Animator animator;
    private int sceneToLoad = -1;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void NextScene()
    {
        int index = nextScene;

        if (relative)
        {
            index += SceneManager.GetActiveScene().buildIndex;
        }

        GotoScene(index);
    }

    public void RestartScene()
    {
        GotoScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void GotoScene(int index)
    {
        if (sceneToLoad == -1) // don't override ongoing transition
        {
            sceneToLoad = index;
            animator.SetTrigger("next");
        }
    }

    public void LoadNewScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public static SceneTransition Find()
    {
        return GameObject.FindGameObjectWithTag("SceneTransition")?.GetComponent<SceneTransition>();
    }
}
