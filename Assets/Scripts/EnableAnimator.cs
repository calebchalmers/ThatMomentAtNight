using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableAnimator : MonoBehaviour
{
    void Start()
    {
        GetComponent<Animator>().enabled = true;
    }
}
