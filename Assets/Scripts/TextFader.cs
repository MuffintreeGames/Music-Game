using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextFader : MonoBehaviour
{
    static float fadeTime = 0.5f;

    bool activated = false;
    TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (activated && text.color.a < 1f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime / fadeTime));
        } else if (!activated && text.color.a > 0f) 
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime / fadeTime));
        }
    }

    public void FadeIn()
    {
        activated = true;
    }

    public void FadeOut()
    {
        activated = false;
    }
}
