using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextAnimation : MonoBehaviour
{
    public float fadeChars = 1f;
    public AnimationCurve fadeCurve = new AnimationCurve(
        new Keyframe(0f, 0f, 0f, 1f),
        new Keyframe(1f, 1f, 1f, 0f)
    );

    [Range(0f, 1f)]
    public float progress = 0f;

    private string text;
    private TextMeshPro mesh;

    void Start()
    {
        mesh = GetComponent<TextMeshPro>();
        text = mesh.text;
    }

    void Update()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        float n = text.Length;
        float t = progress * ((n - 1) / fadeChars + 1);

        string newText = "";
        for (int i = 0; i < n; i++)
        {
            char c = text[i];
            float a = fadeCurve.Evaluate(t - i / fadeChars);
            int h = (int)(a * 255f);
            newText += $"<alpha=#{h:X2}>{c}";
        }
        mesh.text = newText;
    }

    public void ChangeText(string newText)
    {
        text = newText;
        UpdateText();
    }
}
