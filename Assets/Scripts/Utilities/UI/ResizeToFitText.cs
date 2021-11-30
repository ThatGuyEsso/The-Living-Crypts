using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.UI;
public class ResizeToFitText : MonoBehaviour
{
    [SerializeField] private Vector2 padding;
    [SerializeField] private TextMeshProUGUI displayText;
    [SerializeField] private RectTransform elementToFit;
    public void FitText()
    {


        displayText.ForceMeshUpdate();
        Vector2 renderBounds = displayText.GetRenderedValues(false);

        elementToFit.sizeDelta = renderBounds + padding;
    }

}
