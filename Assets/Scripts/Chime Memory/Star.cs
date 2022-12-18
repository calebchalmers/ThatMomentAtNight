using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public bool isReady = false;

    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        gameObject.SetActive(false);
    }

    public void MakeReady()
    {
        animator.SetBool("ready", true);
        isReady = true;
    }

    public void OnClick()
    {
        animator.SetBool("clicked", true);
        animator.SetBool("ready", false);
        isReady = false;
    }
}
