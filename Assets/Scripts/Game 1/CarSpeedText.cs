using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CarSpeedText : MonoBehaviour
{
    public Car car;
    public float scale = 1f;
    public string format = "0";

    private TextMeshProUGUI text;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void LateUpdate()
    {
        text.text = (car.Speed() * scale).ToString(format);
    }
}
