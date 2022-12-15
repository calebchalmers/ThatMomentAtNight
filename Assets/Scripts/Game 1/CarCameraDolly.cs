using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCameraDolly : MonoBehaviour
{
    public Car car;

    private float lastSpeed = 0f;

    void LateUpdate()
    {
        var pos = transform.position;

        if (car.HasCrashed())
        {
            pos.y -= lastSpeed * Time.deltaTime;
        }
        else
        {
            lastSpeed = car.Speed();
            pos.y = car.transform.position.y;
        }
        pos.y = car.transform.position.y;

        transform.position = pos;
    }
}
