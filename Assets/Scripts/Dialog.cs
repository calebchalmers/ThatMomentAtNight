using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
    public float charsPerSec = 10f;
    public float skipRate = 4f;
    public float fadeTime = 0.5f;
    public float waitTime = 0.5f;
    public bool changeSceneAfter = false;

    public string[] lines;
    public AudioClip[] voiceClips;
    public TextMeshPro text;
    public TextAnimation textAnimation;

    private AudioSource audioSource;
    private SceneTransition sceneTransition;
    private EscapeMenu escapeMenu;
    private Animator animator;
    private int lineIndex = -1;
    private bool animating = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        sceneTransition = SceneTransition.Find();
        escapeMenu = FindObjectOfType<EscapeMenu>();
        animator = GetComponent<Animator>();

        animator.SetBool("start", true);
        InputLocker.Lock("Dialog");
        NextLine();
    }

    void Update()
    {
        if (!animating &&
            !escapeMenu.IsShowing() &&
            Input.GetButtonDown("Advance Memory"))
        {
            NextLine();
        }
    }

    private void NextLine()
    {
        lineIndex += 1;

        if (lineIndex == lines.Length)
        {
            StartClosing();
        }
        else
        {
            animating = true;
            StartCoroutine("AnimateFadeOut");
        }
    }

    private void StartClosing()
    {
        this.enabled = false;

        if (changeSceneAfter)
        {
            sceneTransition.NextScene();
            return;
        }

        animator.enabled = true;
        animator.SetBool("end", true);
    }

    private void OnReady()
    {
        animator.enabled = false;
    }

    private void OnClosed()
    {
        InputLocker.Unlock("Dialog");
    }

    private IEnumerator AnimateTextProgress()
    {
        string line = lines[lineIndex];
        int charCount = line.Length;
        textAnimation.progress = 0f;
        textAnimation.ChangeText(line);
        SetOpacity(1f);

        while (true)
        {
            float skipTerm = Input.GetButton("Advance Memory") ? skipRate : 1f;
            textAnimation.progress += Time.deltaTime / charCount * charsPerSec * skipTerm;

            if (textAnimation.progress >= 1f)
                break;

            yield return null;
        }

        textAnimation.progress = 1f;
        animating = false;
    }

    private IEnumerator AnimateFadeOut()
    {
        float opacity = 1f;

        while (true)
        {
            opacity -= Time.deltaTime / fadeTime;

            if (opacity > 0f)
                SetOpacity(opacity);
            else
                break;

            yield return null;
        }

        SetOpacity(0f);
        yield return new WaitForSeconds(waitTime);
        audioSource.PlayOneShot(voiceClips[lineIndex]);
        StartCoroutine("AnimateTextProgress");
    }

    private void SetOpacity(float opacity)
    {
        Color c = text.color;
        c.a = opacity;
        text.color = c;
    }
}
