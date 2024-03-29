using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkippableScene : MonoBehaviour
{
    public bool locked = true;

    private SceneTransition sceneTransition;

    void Start()
    {
        sceneTransition = SceneTransition.Find();
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (!locked)
            {
                sceneTransition.NextScene();
            }
        }
    }

    public void Unlock()
    {
        locked = false;
    }
}
