using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
    public float charsPerSec = 10f;
    public float fadeTime = 0.5f;
    public float waitTime = 0.5f;

    public string[] lines;
    public TextMeshPro text;
    public TextAnimation textAnimation;

    private EscapeMenu escapeMenu;
    private Animator animator;
    public int lineIndex = -1;
    public bool animating = false;

    void Start()
    {
        escapeMenu = FindObjectOfType<EscapeMenu>();
        animator = GetComponent<Animator>();

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
            End();
        }
        else
        {
            animating = true;
            StartCoroutine("AnimateFadeOut");
        }
    }

    private void End()
    {
        animator.enabled = true;
        this.enabled = false;
    }

    private void UnlockInput()
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
            textAnimation.progress += Time.deltaTime / charCount * charsPerSec;

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
        StartCoroutine("AnimateTextProgress");
    }

    private void SetOpacity(float opacity)
    {
        Color c = text.color;
        c.a = opacity;
        text.color = c;
    }
}
