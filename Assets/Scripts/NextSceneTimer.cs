using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextSceneTimer : MonoBehaviour
{
    public float waitTime = 10f;
    public bool restart = false;

    private SceneTransition sceneTransition;

    void Start()
    {
        sceneTransition = SceneTransition.Find();
        Invoke("Elapsed", waitTime);
    }

    private void Elapsed()
    {
        if (restart)
        {
            sceneTransition.RestartScene();
        }
        else
        {
            sceneTransition.NextScene();
        }
    }
}
