using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public float moveSpeed = 0.5f;
    public AnimationCurve rotateSpeedCurve;
    public float wobbleAmplitude = 20f;
    public float wobbleWavelength = 2f;
    public BansheeGz.BGSpline.Components.BGCcMath followSpline = null;

    private TrailRenderer trailRenderer;
    private Player player;
    private float pathDistance;
    private Vector2 targetPoint;

    void Start()
    {
        trailRenderer = GetComponentInChildren<TrailRenderer>();
        player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();

        if (followSpline != null)
        {
            pathDistance = followSpline.GetDistance();
            FollowSpline();
            trailRenderer.Clear();
        }

        // if (player != null)
        // {
        //     player.OnHide += () =>
        //     {
        //         randomTarget = Random.insideUnitCircle.normalized * 100f;
        //     };
        // }
    }

    void Update()
    {
        if (followSpline != null)
        {
            FollowSpline();
        }
        else if (player != null)
        {
            TrackPlayer();
        }
    }

    private void FollowSpline()
    {
        float dist = (Time.timeSinceLevelLoad * moveSpeed) % pathDistance;
        float wobbleTime = Time.time;
        float wobble = Mathf.Sin(wobbleTime / wobbleWavelength * Mathf.PI * 2f) * wobbleAmplitude;
        Vector3 basePos = followSpline.CalcPositionAndTangentByDistance(dist, out Vector3 tangent);
        Vector3 normal = new Vector3(-tangent.y, tangent.x, 0f);
        Vector3 wobblePos = normal * wobble;

        float baseAngle = Mathf.Atan2(tangent.y, tangent.x) * Mathf.Rad2Deg;
        float a = Mathf.PI * 2f / wobbleWavelength;
        float wobbleAngle = Mathf.Atan(a * wobbleAmplitude * Mathf.Cos(a * wobbleTime)) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0f, 0f, baseAngle + wobbleAngle);
        transform.position = basePos + wobblePos;
    }

    private void TrackPlayer()
    {
        if (player.isHiding)
        {
            targetPoint += Random.insideUnitCircle;
        }
        else
        {
            targetPoint = player.transform.position - transform.position;
        }

        float distance = targetPoint.magnitude;

        float currentAngle = transform.eulerAngles.z;
        float targetAngle = Mathf.Atan2(targetPoint.y, targetPoint.x) * Mathf.Rad2Deg;
        float deltaAngle = Mathf.DeltaAngle(currentAngle, targetAngle);
        float rotateSpeed = rotateSpeedCurve.Evaluate(Mathf.Abs(deltaAngle)) * Time.deltaTime;

        float wobble = Mathf.Sin(Time.time / wobbleWavelength * Mathf.PI * 2f) * wobbleAmplitude;
        float newAngle = currentAngle + Mathf.Clamp(deltaAngle + wobble, -rotateSpeed, rotateSpeed);

        transform.eulerAngles = new Vector3(0f, 0f, newAngle);
        transform.Translate(moveSpeed * Time.deltaTime, 0f, 0f, Space.Self);
    }
}
