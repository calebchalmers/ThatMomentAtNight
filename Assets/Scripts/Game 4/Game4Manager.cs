using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game4Manager : MonoBehaviour
{
    public Bar[] bars;
    public GameObject[] breaths;

    private int hits = 0;
    private SceneTransition sceneTransition;

    private void Awake()
    {
        foreach (var bar in bars)
        {
            bar.SetManager(this);
        }
    }

    void Start()
    {
        sceneTransition = SceneTransition.Find();
    }

    public void Success()
    {
        if (hits < breaths.Length)
        {
            breaths[hits].SetActive(true);
        }

        hits++;

        if (hits == bars.Length)
        {
            Won();
        }
        else
        {
            bars[hits].gameObject.SetActive(true);
        }
    }

    public void Won()
    {
        sceneTransition.NextScene();
    }

    public void GameOver()
    {
        sceneTransition.RestartScene();
    }
}
