using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public float triggerRadius = 6f;

    private bool triggered = false;
    private Animator animator;
    private Car car;

    void Start()
    {
        animator = GetComponent<Animator>();
        car = FindObjectOfType<Car>();

        if (Random.value > 0.5f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    void Update()
    {
        if (!triggered)
        {
            var diff = (car.transform.position - transform.position);
            if (diff.magnitude < triggerRadius)
            {
                Trigger();
            }
        }
    }

    private void Trigger()
    {
        triggered = true;
        animator.SetBool("triggered", true);
    }

    private void OnDrawGizmos()
    {
        Vector3 pos = transform.position;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(pos + new Vector3(-1f, triggerRadius, 0f), pos + new Vector3(1f, triggerRadius, 0f));
    }
}
