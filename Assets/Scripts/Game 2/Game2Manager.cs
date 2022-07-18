using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game2Manager : MonoBehaviour
{
    public LayerMask starLayerMask;
    public Bell[] bells;

    private int starsClicked = 0;
    private SceneTransition sceneTransition;

    void Start()
    {
        sceneTransition = SceneTransition.Find();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckForStars();
        }
    }

    private void CheckForStars()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hits = Physics2D.RaycastAll(ray.origin, ray.direction, Mathf.Infinity, starLayerMask);

        foreach (var hit in hits)
        {
            ClickedStar(hit.collider.gameObject);
        }
    }

    private void ClickedStar(GameObject obj)
    {
        bells[starsClicked++].Ring();
        Destroy(obj);

        if (starsClicked == bells.Length)
        {
            sceneTransition.NextScene();
        }
    }
}
