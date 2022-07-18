using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    public BoxCollider2D trigger;
    public float useTime = 10f;

    private Player player;
    private float timeLeft;
    private bool oldPlayerInside = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();
        timeLeft = useTime;
    }

    void Update()
    {
        bool playerInside = trigger.OverlapPoint(player.transform.position);

        if (oldPlayerInside != playerInside)
        {
            player.SetHiding(playerInside);
        }

        oldPlayerInside = playerInside;

        if (playerInside)
        {
            timeLeft -= Time.deltaTime;

            if (timeLeft < 0.0f)
            {
                Break();
            }
        }
    }

    private void Break()
    {
        if (oldPlayerInside)
        {
            player.SetHiding(false);
        }

        Destroy(gameObject);
    }
}
