using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainPanel;

    [Header("Controls")]
    public Button continueButton;
    public GameObject quitButton;

    [Header("Slide")]
    public RectTransform slideTransform;
    public float slideLerpSpeed = 60f;

    [Header("Misc")]
    public AudioMixer mixer;

    private SceneTransition sceneTransition;

    void Start()
    {
        sceneTransition = SceneTransition.Find();
        continueButton.interactable = PlayerPrefs.HasKey("scene");

#if !(UNITY_EDITOR || UNITY_STANDALONE)
        quitButton.SetActive(false);
#endif
    }

    void Update()
    {
        EventSystem eventSystem = EventSystem.current;
        PointerEventData pointerData = new PointerEventData(eventSystem);
        pointerData.Reset();
        pointerData.position = Input.mousePosition;
        List<RaycastResult> result = new List<RaycastResult>();
        eventSystem.RaycastAll(pointerData, result);

        foreach (var hit in result)
        {
            var selectable = hit.gameObject.GetComponent<Selectable>();
            if (selectable != null && selectable.interactable)
            {
                selectable.Select();
            }
        }

        var selected = eventSystem.currentSelectedGameObject;
        if (selected != null)
        {
            Vector3 pos = slideTransform.localPosition;
            pos.y = Mathf.Lerp(
                pos.y,
                selected.transform.localPosition.y,
                slideLerpSpeed * Time.deltaTime
            );
            slideTransform.localPosition = pos;
        }
    }

    public void OnPressedNewGame()
    {
        sceneTransition.NextScene();
    }

    public void OnPressedContinue()
    {
        int savedScene = PlayerPrefs.GetInt("scene");
        sceneTransition.GotoScene(savedScene);
    }

    public void OnPressedQuit()
    {
        Application.Quit();
    }
}
