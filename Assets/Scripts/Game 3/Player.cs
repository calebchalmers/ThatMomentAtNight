using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float maxMoveSpeed = 1.0f;
    public float airMoveAccel = 1.0f;
    public float airDrag = 200.0f;
    public float jumpSpeed = 1.0f;
    public float gravity = -10.0f;
    public bool alwaysJump = false;
    public bool alwaysUseMouse = false;
    public AnimationCurve mouseMoveCurve;
    public float rotationFactor = 1.0f;

    [Header("Crates")]
    public Transform crateTarget;
    public LayerMask crateLayerMask;
    public float crateSpeedFactor = 0.5f;

    [Header("Other")]
    public LayerMask groundLayerMask;
    public int defaultLayer = 0;
    public int jumpingLayer = 0;
    public Transform colliderTransform;
    public Transform spriteTransform;
    public SceneTransition sceneTransition;

    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 velocity;
    private bool isGrounded;
    public bool isHiding = false;
    private Rigidbody2D holding = null;

    private const float GroundedVelocityThreshold = 0.01f;
    private const float GroundedRaycastDistance = 0.04f;
    private const float MovementThreshold = 0.01f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        CheckIfGrounded();
        // UpdateCrateLogic();
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        bool isCrouching = Input.GetButton("Crouch");
        velocity = rb.velocity;

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        // Handle manual movement
        float moveDir = Input.GetAxisRaw("Horizontal");
        float moveSpeed = maxMoveSpeed * (holding != null ? crateSpeedFactor : 1f);

        if (alwaysUseMouse || Input.GetMouseButton(0))
        {
            float mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
            float mouseDeltaX = mouseWorldPos - transform.position.x;
            moveDir = Mathf.Sign(mouseDeltaX) * mouseMoveCurve.Evaluate(Mathf.Abs(mouseDeltaX));
        }

        if (isGrounded)
        {
            velocity.x = moveDir * moveSpeed;

            bool canJump = !isCrouching && holding == null;
            if (canJump && (alwaysJump || Input.GetButton("Jump")))
            {
                velocity.y = jumpSpeed;
                animator.SetTrigger("Jump");
            }
        }
        else
        {
            if (Mathf.Abs(moveDir) > MovementThreshold)
            {
                velocity.x += moveDir * airMoveAccel * Time.deltaTime;
                velocity.x = Mathf.Clamp(velocity.x, -moveSpeed, moveSpeed);
            }
            else
            {
                // Constant deceleration
                // velocity.x = Mathf.Sign(velocity.x) * 
                //     Mathf.Max(Mathf.Abs(velocity.x) - airDecel * Time.deltaTime, 0.0f);

                // Exponential deceleration
                velocity.x *= Mathf.Pow(1.0f / airDrag, Time.deltaTime);
            }
        }

        bool canTouchPlatforms = velocity.y > GroundedVelocityThreshold || isCrouching;
        colliderTransform.gameObject.layer = canTouchPlatforms ? jumpingLayer : defaultLayer;
        spriteTransform.localEulerAngles = new Vector3(0.0f, 0.0f, -velocity.x * rotationFactor);

        rb.velocity = velocity;
    }

    private void CheckIfGrounded()
    {
        isGrounded = false;
        if (rb.velocity.y < GroundedVelocityThreshold)
        {
            var groundHit = Physics2D.BoxCast(
                colliderTransform.position, colliderTransform.lossyScale,
                0f, Vector2.down, GroundedRaycastDistance,
                groundLayerMask
            );
            isGrounded = (groundHit.collider != null);
        }
    }

    // private void UpdateCrateLogic()
    // {
    //     if (Input.GetButtonDown("Crouch"))
    //     {
    //         if (holding == null)
    //         {
    //             var hit = Physics2D.OverlapPoint(transform.position, crateLayerMask);
    //             if (hit != null)
    //             {
    //                 holding = hit.attachedRigidbody;
    //             }
    //         }
    //         else
    //         {
    //             holding = null;
    //         }
    //     }

    //     if (Input.GetButtonDown("Jump"))
    //     {
    //         holding = null;
    //     }

    //     if (holding != null)
    //     {
    //         holding.MovePosition(crateTarget.position);
    //     }
    // }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hand") && !isHiding)
        {
            sceneTransition.RestartScene();
        }
    }

    public void SetHiding(bool hiding)
    {
        this.isHiding = hiding;
    }
}
