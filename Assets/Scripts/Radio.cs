using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Radio : MonoBehaviour
{
    public string[] titles;
    public AudioClip[] songs;
    public int songIndex = 0;
    public TextMeshPro text;
    public AudioSource songSource;
    public AudioSource noiseSource;

    void Start()
    {
        ChangeSong(songIndex, false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Change Song"))
        {
            ChangeSong((songIndex + 1) % titles.Length);
        }
    }

    private void ChangeSong(int index, bool makeNoise = true)
    {
        songIndex = index;
        text.text = titles[index];

        if (makeNoise)
            noiseSource.Play();

        songSource.Stop();
        songSource.clip = songs[index];
        songSource.Play();
    }
}
