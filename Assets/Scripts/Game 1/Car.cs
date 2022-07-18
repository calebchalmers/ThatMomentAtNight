using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float horizontalLimit = 1f;
    public float horizontalSpeed = 5f;
    public float verticalSpeed = -5;
    public float maxAngle = 15f;
    public float angleLerpSpeed = 20f;

    private SceneTransition sceneTransition;
    private Rigidbody2D rb;
    private bool crashed = false;

    void Start()
    {
        sceneTransition = SceneTransition.Find();
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (!crashed)
        {
            Vector3 pos = rb.position;
            float velX = Input.GetAxisRaw("Horizontal") * horizontalSpeed * Time.fixedDeltaTime;
            pos.x = Mathf.Clamp(pos.x + velX, -horizontalLimit, horizontalLimit);
            pos.y += verticalSpeed * Time.deltaTime;
            rb.MovePosition(pos);

            float angle = Input.GetAxisRaw("Horizontal") * maxAngle;
            rb.MoveRotation(Mathf.Lerp(rb.rotation, angle, angleLerpSpeed * Time.fixedDeltaTime));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("FinishLine"))
        {
            sceneTransition.NextScene();
        }
        else
        {
            crashed = true;
            sceneTransition.RestartScene();
        }
    }
}
