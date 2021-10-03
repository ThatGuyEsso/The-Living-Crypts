using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class FadeOutImages : MonoBehaviour
{
    private Image[] images;
    private TextMeshProUGUI[] textElements;
    private float fadeOutRate;
    private float fadeInRate;
    bool isFadingIn;
    bool isFadingOut;
    public System.Action OnFadeComplete;


    public void Awake()
    {
        images = GetComponentsInChildren<Image>();
        textElements = GetComponentsInChildren<TextMeshProUGUI>();
    }

    public void ShowSprite()
    {
        isFadingIn = false;
        isFadingOut = false;
        foreach(Image image in images)
        {

            image.color = new Vector4(image.color.r,
             image.color.g, image.color.b, 1f);
        }

        foreach (TextMeshProUGUI text in textElements)
        {

            text.color = new Vector4(text.color.r,
             text.color.g, text.color.b, 1f);
        }
    }
    public void BeginFadeOut(float rate)
    {
        fadeOutRate = rate;
        isFadingIn = false;
        isFadingOut = true;
    }
    public void BeginFadeIn(float rate)
    {
        if(images !=null &&images.Length > 0)
        {
            foreach (Image image in images)
            {

                image.color = new Vector4(image.color.r,
                 image.color.g, image.color.b, 0f);
            }
        }
       
        if (textElements !=null && textElements.Length > 0)
        {
            foreach (TextMeshProUGUI text in textElements)
            {

                text.color = new Vector4(text.color.r,
                 text.color.g, text.color.b, 0f);
            }

        }

        fadeInRate = rate;
        isFadingOut = false;
        isFadingIn = true;
 
    }



    public void DoFadeOut()
    {
        foreach (Image image in images)
        {

            image.color = Vector4.Lerp(image.color, new Vector4(image.color.r,
                image.color.g, image.color.b, 0f),Time.deltaTime*fadeOutRate);
            if (image.color.a <= 0.05)
            {
                image.color = new Vector4(image.color.r,
                image.color.g, image.color.b, 0f);
                isFadingOut = false;
              
            }
        }

        foreach (TextMeshProUGUI text in textElements)
        {
            text.color = Vector4.Lerp(text.color, new Vector4(text.color.r,
            text.color.g, text.color.b, 0f), Time.deltaTime * fadeOutRate);
            if (text.color.a <= 0.05)
            {
                text.color = new Vector4(text.color.r,
                text.color.g, text.color.b, 0f);
                isFadingOut = false;
                OnFadeComplete?.Invoke();
            }

        }

    }
    public void DoFadeIn()
    {
        foreach (Image image in images)
        {
            image.color = Vector4.Lerp(image.color, new Vector4(image.color.r,
            image.color.g, image.color.b, 1f), Time.deltaTime * fadeInRate);
            if (image.color.a >= 0.95)
            {
                image.color = new Vector4(image.color.r,
                image.color.g, image.color.b, 1f);
                isFadingIn = false;

            }
        }

        foreach (TextMeshProUGUI text in textElements)
        {
            text.color = Vector4.Lerp(text.color, new Vector4(text.color.r,
            text.color.g, text.color.b, 1f), Time.deltaTime * fadeInRate);
            if (text.color.a >= 0.95)
            {
                text.color = new Vector4(text.color.r,
                text.color.g, text.color.b, 1f);
                isFadingIn = false;
                OnFadeComplete?.Invoke();
            }
        }



    }
    public void Update()
    {
        if(isFadingOut && !isFadingIn)
        {
            DoFadeOut();
        }
        else if(!isFadingOut && isFadingIn)
        {
            DoFadeIn();
        }
    }


    private void OnDisable()
    {
        isFadingIn = false;
        isFadingOut = false;
    }
}
