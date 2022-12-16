using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingMaterial : MonoBehaviour
{
    public float factor = 1.0f;
    public Vector2 constant = Vector2.zero;

    private Material mat;

    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
    }

    void LateUpdate()
    {
        Vector2 offset = new Vector2(transform.position.x, transform.position.y);
        mat.mainTextureOffset = offset * factor + constant;
    }
}
