using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Car : MonoBehaviour
{
    public float linearSpeed = 12f;
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

    [Header("Auto-correction")]
    public Transform[] lanes;
    public float maxAutoCorrectTurnAngle = 5f;
    public float autoCorrectDeltaScale = 10f;

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
            (int closestLane, float laneDX) = ClosestLane(frontWheels.position.x);
            float targetAngle = Mathf.Clamp(
                laneDX * autoCorrectDeltaScale,
                -maxAutoCorrectTurnAngle,
                maxAutoCorrectTurnAngle
            );

            float diff = targetAngle - (carAngle + wheelAngle);
            float delta = Mathf.Sign(diff) * Mathf.Min(Mathf.Abs(diff), turnSpeed * Time.fixedDeltaTime);
            wheelAngle += delta;
        }

        float globalTurnAngle = (wheelAngle + carAngle) * Mathf.Deg2Rad;

        Vector3 turnDir = new Vector3(Mathf.Sin(globalTurnAngle), -Mathf.Cos(globalTurnAngle), 0f);
        // Vector3 turnDir = new Vector3(Mathf.Sin(globalTurnAngle), -Mathf.Cos(globalTurnAngle), 0f);
        Vector3 pretend = pos + new Vector3(Mathf.Sin(carAngle * Mathf.Deg2Rad), -Mathf.Cos(carAngle * Mathf.Deg2Rad), 0f) * frontWheelDistance + turnDir * linearSpeed * Time.fixedDeltaTime;
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
            linearSpeed *= 0.5f;
            crashSound.SetActive(true);
            Invoke("RestartScene", restartDelay);
        }
    }

    private void RestartScene()
    {
        sceneTransition.RestartScene();
    }

    private (int, float) ClosestLane(float x)
    {
        float minDist = Mathf.Infinity;
        float minDX = Mathf.Infinity;
        int minIndex = 0;

        for (int i = 0; i < lanes.Length; i++)
        {
            float laneX = lanes[i].position.x;
            float dx = laneX - x;
            float dist = Mathf.Abs(dx);
            if (dist < minDist)
            {
                minDist = dist;
                minDX = dx;
                minIndex = i;
            }
        }

        return (minIndex, minDX);
    }

    public bool HasCrashed()
    {
        return crashed;
    }
}
