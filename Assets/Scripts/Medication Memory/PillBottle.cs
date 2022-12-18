using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillBottle : MonoBehaviour
{
    public ParticleSystem[] particles;
    public int pillCount = 10;

    private Animator animator;
    private SceneTransition sceneTransition;

    void Start()
    {
        animator = GetComponent<Animator>();
        sceneTransition = SceneTransition.Find();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shake();
        }
    }

    private void Shake()
    {
        animator.SetTrigger("shake");
        pillCount -= 1;

        if (pillCount == 0)
        {
            sceneTransition.NextScene();
        }
    }

    public void ReleasePill()
    {
        int i = Random.Range(0, particles.Length);
        particles[i].Emit(1);
    }
}
