using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Radio : MonoBehaviour
{
    public string[] titles;
    public int songIndex = 0;
    public TextMeshPro text;

    void Start()
    {
        ChangeSong(songIndex);
    }

    void Update()
    {
        if (Input.GetButtonDown("Change Song"))
        {
            ChangeSong((songIndex + 1) % titles.Length);
        }
    }

    private void ChangeSong(int index)
    {
        songIndex = index;
        text.text = titles[index];
    }
}
