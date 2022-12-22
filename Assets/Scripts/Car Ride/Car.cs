using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.Linq;

public class Car : MonoBehaviour
{
    public float speed = 0f;
    public float minSpeed = 5f;
    public float maxSpeed = 30f;
    public float accel = 1.2f;
    public float brakeDecel = 4f;
    public float crashDecel = 20f;
    public float maxTurnAngle = 15f;
    public float turnSpeed = 150f;
    public float restartDelay = 1.0f;
    public Transform frontWheels;

    [Header("Crash")]
    public int crashSceneIndex;
    public GameObject crashSound;
    public AudioMixer audioMixer;
    public AudioMixerSnapshot crashMixerSnapshot;

    private SceneTransition sceneTransition;
    private Rigidbody2D rb;
    private Animator animator;
    private bool crashed = false;
    private bool inputLocked;
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
        animator = GetComponent<Animator>();
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
        if (InputLocker.IsLocked)
        {
            return;
        }

        float dt = Time.fixedDeltaTime;
        float carAngle = rb.rotation;
        Vector3 pos = rb.position;
        float effTurnSpeed = turnSpeed / speed; // better handling

        bool turning = inputDir != 0f;

        if (turning)
        {
            float angleVel = inputDir * effTurnSpeed * dt;
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
            float delta = Mathf.Sign(diff) * Mathf.Min(Mathf.Abs(diff), effTurnSpeed * dt);
            wheelAngle += delta;
        }

        float globalTurnAngle = (wheelAngle + carAngle) * Mathf.Deg2Rad;

        Vector3 turnDir = new Vector3(Mathf.Sin(globalTurnAngle), -Mathf.Cos(globalTurnAngle), 0f);
        // Vector3 turnDir = new Vector3(Mathf.Tan(globalTurnAngle), -1f, 0f);
        Vector3 pretend = pos + turnDir * speed * dt + frontWheelDistance *
            new Vector3(Mathf.Sin(carAngle * Mathf.Deg2Rad), -Mathf.Cos(carAngle * Mathf.Deg2Rad), 0f);
        Vector3 carDir = (pretend - pos).normalized;
        pos = pretend - carDir * frontWheelDistance;
        carAngle = Mathf.Atan2(carDir.x, -carDir.y) * Mathf.Rad2Deg;

        rb.MoveRotation(carAngle);
        rb.MovePosition(pos);

        // Accelerate/decelerate
        if (crashed)
        {
            speed = Mathf.Max(0.0f, speed - crashDecel * dt);
        }
        else
        {
            bool isBraking = Input.GetButton("Brake");
            speed += (isBraking ? -brakeDecel : accel) * dt;
            speed = Mathf.Clamp(speed, minSpeed, maxSpeed);
        }
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
            speed *= 0.5f;
            crashSound.SetActive(true);
            animator.SetBool("crashed", true);
            // audioMixer.TransitionToSnapshots(new AudioMixerSnapshot[] { crashMixerSnapshot }, new float[] { 1.0f }, 0.25f);
            Invoke("RestartScene", restartDelay);
        }
    }

    private void RestartScene()
    {
        sceneTransition.GotoScene(crashSceneIndex);
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

    public float Speed()
    {
        return speed;
    }
}
