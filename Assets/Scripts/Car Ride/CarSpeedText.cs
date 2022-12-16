using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CarSpeedText : MonoBehaviour
{
    public float scale = 1f;
    public string format = "0";

    private Car car;
    private TextMeshPro text;

    void Start()
    {
        car = FindObjectOfType<Car>();
        text = GetComponent<TextMeshPro>();
    }

    void LateUpdate()
    {
        text.text = (car.Speed() * scale).ToString(format);
    }
}
