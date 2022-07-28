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
    public GameObject settingsPanel;

    [Header("Controls")]
    public Button continueButton;
    public GameObject quitButton;
    public Slider volumeSlider;

    [Header("Flashlight")]
    public RectTransform flashlightTransform;
    public float flashlightLerpSpeed = 60f;

    [Header("Misc")]
    public AudioMixer mixer;

    private SceneTransition sceneTransition;

    void Start()
    {
        sceneTransition = SceneTransition.Find();

#if UNITY_EDITOR || UNITY_STANDALONE
        quitButton.SetActive(true);
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
            Vector3 pos = flashlightTransform.position;
            pos.y = Mathf.Lerp(
                pos.y,
                selected.transform.position.y,
                flashlightLerpSpeed * Time.deltaTime
            );
            flashlightTransform.position = pos;
        }
    }

    public void OnPressedNewGame()
    {
        sceneTransition.NextScene();
    }

    public void OnPressedContinue()
    {

    }

    public void OnPressedOpenSettings()
    {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void OnPressedCloseSettings()
    {
        mainPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void OnPressedQuit()
    {
        Application.Quit();
    }

    public void OnVolumeChanged()
    {
        mixer.SetFloat("Volume", Mathf.Lerp(-80f, 0f, volumeSlider.value));
    }
}
