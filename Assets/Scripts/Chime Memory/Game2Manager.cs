using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Game2Manager : MonoBehaviour
{
    public LayerMask starLayerMask;
    public Star[] stars;
    public Bell[] bells;

    private int starsClicked = 0;
    private SceneTransition sceneTransition;

    void Start()
    {
        sceneTransition = SceneTransition.Find();

        stars[0].gameObject.SetActive(true);
        stars[1].gameObject.SetActive(true);
        stars[0].MakeReady();
    }

    void Update()
    {
        if (!InputLocker.IsLocked)
        {
            if (Input.GetMouseButtonDown(0))
            {
                CheckStars();
            }

            if (Input.GetButtonDown("Advance Memory"))
            {
                ClickedStar(stars[starsClicked]);
            }
        }
    }

    private void CheckStars()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hits = Physics2D.RaycastAll(ray.origin, ray.direction, Mathf.Infinity, starLayerMask);

        foreach (var hit in hits)
        {
            GameObject obj = hit.collider.gameObject;
            Star star = obj.GetComponent<Star>();

            if (star.isReady)
            {
                ClickedStar(star);
            }
        }
    }

    private void ClickedStar(Star star)
    {
        star.OnClick();
        bells.ElementAtOrDefault(starsClicked)?.Ring();
        stars.ElementAtOrDefault(starsClicked + 1)?.MakeReady();
        stars.ElementAtOrDefault(starsClicked + 2)?.gameObject?.SetActive(true);

        starsClicked += 1;

        if (starsClicked == stars.Length)
        {
            sceneTransition.NextScene();
        }
    }
}
