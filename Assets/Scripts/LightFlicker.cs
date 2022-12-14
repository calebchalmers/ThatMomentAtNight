using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public GameObject lightObject;
    public float chance = 0.1f;
    public float duration = 0.1f;

    void Start()
    {

    }

    void Update()
    {
        if (Random.value < chance * Time.deltaTime)
        {
            StartCoroutine("Flicker");
        }
    }

    IEnumerator Flicker()
    {
        lightObject.SetActive(false);
        yield return new WaitForSeconds(duration);
        lightObject.SetActive(true);
    }
}
