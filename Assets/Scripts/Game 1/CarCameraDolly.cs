using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCameraDolly : MonoBehaviour
{
    public Car car;

    private float crashSpeed;

    void Start()
    {
        crashSpeed = car.linearSpeed;
    }

    void LateUpdate()
    {
        var pos = transform.position;

        if (car.HasCrashed())
        {
            pos.y -= crashSpeed * Time.deltaTime;
        }
        else
        {
            pos.y = car.transform.position.y;
        }

        transform.position = pos;
    }
}
