using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingMaterial : MonoBehaviour
{
    public float factor = 1.0f;

    private Material mat;

    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
    }

    void LateUpdate()
    {
        mat.mainTextureOffset = transform.position * factor;
    }
}
