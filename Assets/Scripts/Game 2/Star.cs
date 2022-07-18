using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public float activateTime = 0f;

    void Start()
    {
        gameObject.SetActive(false);
        Invoke("Activate", activateTime);
    }

    private void Activate()
    {
        gameObject.SetActive(true);
    }
}
