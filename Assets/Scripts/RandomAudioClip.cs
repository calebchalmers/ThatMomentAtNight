using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAudioClip : MonoBehaviour
{
    public float minPitch = 1f;
    public float maxPitch = 1f;

    public AudioClip[] clips;

    void Start()
    {
        int i = Random.Range(0, clips.Length);
        var source = GetComponent<AudioSource>();
        source.clip = clips[i];
        source.pitch = Random.Range(minPitch, maxPitch);
        source.Play();
    }
}
