using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memory : MonoBehaviour
{
    private Animator animator;
    private SceneTransition sceneTransition;

    void Start()
    {
        animator = GetComponent<Animator>();
        sceneTransition = SceneTransition.Find();
    }

    void Update()
    {
        animator.SetBool("continue", Input.GetKeyDown(KeyCode.Space));
    }

    public void EndScene()
    {
        sceneTransition.NextScene();
    }
}
