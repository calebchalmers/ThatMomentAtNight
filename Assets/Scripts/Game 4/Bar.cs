using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bar : MonoBehaviour
{
    public float speed = 1f;
    public Transform barTransform;
    public Transform targetTransform;
    public Transform knobTransform;

    private float seedTime;
    private Game4Manager manager;

    void Start()
    {
        seedTime = Random.value * 2f;
    }

    void Update()
    {
        float barWidth = barTransform.localScale.x;
        float targetWidth = targetTransform.localScale.x;

        float t = Time.time * speed / barWidth + seedTime;
        float x = (Mathf.Abs(((t + 1.5f) % 2f) - 1f) - 0.5f) * barWidth;
        knobTransform.localPosition = new Vector3(x, 0f, 0f);

        bool onTarget = Mathf.Abs(x) < targetWidth / 2f;

        if (Input.GetMouseButtonDown(0))
        {
            Entered(onTarget);
        }
    }

    private void Entered(bool onTarget)
    {
        gameObject.SetActive(false);

        if (onTarget)
        {
            manager.Success();
        }
        else
        {
            manager.GameOver();
        }
    }

    public void SetManager(Game4Manager manager)
    {
        this.manager = manager;
    }
}
