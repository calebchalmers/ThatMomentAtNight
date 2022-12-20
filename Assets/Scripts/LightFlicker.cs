using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public SpriteRenderer lightRenderer;
    public float chance = 0.25f;
    public float duration = 0.03f;
    public float regularOpacity = 1f;
    public float fickerOpacity = 0.8f;

    void Update()
    {
        if (Random.value < chance * Time.deltaTime)
        {
            StartCoroutine("Flicker");
        }
    }

    IEnumerator Flicker()
    {
        Color color = lightRenderer.color;
        color.a = fickerOpacity;
        lightRenderer.color = color;
        yield return new WaitForSeconds(duration);
        color.a = regularOpacity;
        lightRenderer.color = color;
    }
}
