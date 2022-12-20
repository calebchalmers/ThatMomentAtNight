using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memory : MonoBehaviour
{
    private Animator animator;
    private SceneTransition sceneTransition;
    private EscapeMenu escapeMenu;

    void Start()
    {
        animator = GetComponent<Animator>();
        sceneTransition = SceneTransition.Find();
        escapeMenu = FindObjectOfType<EscapeMenu>();
    }

    void Update()
    {
        animator.SetBool("continue",
            Input.GetButtonDown("Advance Memory") && !escapeMenu.IsShowing()
        );
    }

    public void EndScene()
    {
        sceneTransition.NextScene();
    }
}
