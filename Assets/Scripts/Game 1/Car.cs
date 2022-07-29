using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float verticalSpeed = 5;
    public float maxTurnAngle = 15f;
    public float turnSpeed = 150f;
    public float restartDelay = 1.0f;
    public GameObject crashSound;
    public Transform frontWheels;

    private SceneTransition sceneTransition;
    private Rigidbody2D rb;
    private bool crashed = false;
    private float wheelAngle = 0f;
    private float frontWheelDistance;
    private float inputDir = 0f;

    void Start()
    {
        sceneTransition = SceneTransition.Find();
        rb = GetComponent<Rigidbody2D>();
        frontWheelDistance = frontWheels.localPosition.magnitude;
    }

    void Update()
    {
        inputDir = Input.GetAxisRaw("Horizontal");

        Vector3 wheelEuler = frontWheels.localEulerAngles;
        wheelEuler.z = wheelAngle;
        frontWheels.localEulerAngles = wheelEuler;
    }

    void FixedUpdate()
    {
        // Vector3 pos = rb.position;
        // float velX = Input.GetAxisRaw("Horizontal") * horizontalSpeed * Time.fixedDeltaTime;
        // pos.x = Mathf.Clamp(pos.x + velX, -horizontalLimit, horizontalLimit);
        // pos.y += verticalSpeed * Time.deltaTime;
        // rb.MovePosition(pos);

        // if (!crashed)
        // {
        //     float angle = rb.rotation;
        //     Vector3 pos = rb.position;

        //     float turnDir = Input.GetAxisRaw("Horizontal");
        //     bool turning = turnDir != 0f;

        //     if (turning)
        //     {
        //         float angleSpeed = turnCurve.Evaluate(turnDir * angle / maxAngle);
        //         float angleVel = turnDir * angleSpeed * Time.fixedDeltaTime;
        //         angle = Mathf.Clamp(angle + angleVel, -maxAngle, maxAngle);
        //         pos.x += Input.GetAxisRaw("Horizontal") * horizontalSpeed * Time.fixedDeltaTime;
        //     }
        //     else
        //     {
        //         // angle = Mathf.Sign(angle) * Mathf.Max(Mathf.Abs(angle) - angleSpeed * Time.deltaTime, 0f);
        //         angle *= Mathf.Pow(angleDrag, Time.fixedDeltaTime);
        //     }

        //     rb.MoveRotation(angle);

        //     Vector3 dir = new Vector3(Mathf.Tan(angle * Mathf.Deg2Rad), -1f, 0f);
        //     pos += dir * verticalSpeed * Time.fixedDeltaTime;
        //     rb.MovePosition(pos);
        // }


        float carAngle = rb.rotation;
        Vector3 pos = rb.position;

        bool turning = inputDir != 0f;

        if (turning)
        {
            float angleVel = inputDir * turnSpeed * Time.fixedDeltaTime;
            wheelAngle = Mathf.Clamp(wheelAngle + angleVel + carAngle, -maxTurnAngle, maxTurnAngle) - carAngle;
        }
        else
        {
            float diff = (-carAngle) - wheelAngle;
            float delta = Mathf.Sign(diff) * Mathf.Max(Mathf.Abs(diff) - turnSpeed * Time.fixedDeltaTime, 0f);
            wheelAngle = -carAngle - delta;
        }

        float globalTurnAngle = (wheelAngle + carAngle) * Mathf.Deg2Rad;

        Vector3 turnDir = new Vector3(Mathf.Tan(globalTurnAngle), -1f, 0f);
        // Vector3 turnDir = new Vector3(Mathf.Sin(globalTurnAngle), -Mathf.Cos(globalTurnAngle), 0f);
        Vector3 pretend = pos + new Vector3(Mathf.Sin(carAngle * Mathf.Deg2Rad), -Mathf.Cos(carAngle * Mathf.Deg2Rad), 0f) * frontWheelDistance + turnDir * verticalSpeed * Time.fixedDeltaTime;
        Vector3 carDir = (pretend - pos).normalized;
        pos = pretend - carDir * frontWheelDistance;
        carAngle = Mathf.Atan2(carDir.x, -carDir.y) * Mathf.Rad2Deg;

        rb.MoveRotation(carAngle);
        rb.MovePosition(pos);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("FinishLine"))
        {
            sceneTransition.NextScene();
        }
        else if (!crashed)
        {
            crashed = true;
            verticalSpeed *= 0.5f;
            crashSound.SetActive(true);
            Invoke("RestartScene", restartDelay);
        }
    }

    private void RestartScene()
    {
        sceneTransition.RestartScene();
    }
}
