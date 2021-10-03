using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ClampAlpha : MonoBehaviour
{
    private Image image;
    [SerializeField] private float maxAlpha;

    private void Awake()
    {
        image = GetComponent<Image>();
    
    }

    private void LateUpdate()
    {
        if (image.color.a >= maxAlpha) image.color = new Color(image.color.r, image.color.g, image.color.b, maxAlpha);
    }
}
