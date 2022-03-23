using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HUDPrompt : MonoBehaviour
{
    [SerializeField] private float DefaultShowTime=5.0f;
    private ResizeToFitText _resizeToFit;
    private TextMeshProUGUI _promptText;
    private Image _textBackground;


    private void Awake()
    {
        _resizeToFit = GetComponent<ResizeToFitText>();
        _promptText = GetComponentInChildren<TextMeshProUGUI>();
        
        _textBackground = GetComponentInChildren<Image>();
        RemovePrompt();

    }

    public void ShowPrompt(string prompt, Color color)
    {
        if (_promptText)
        {
            _promptText.enabled = true;
        }
        if (_textBackground)
        {
            _textBackground.enabled = true;
        }
        _promptText.text = prompt;
        _promptText.color=color;
        if (_resizeToFit)
        {
            _resizeToFit.FitText();
        }
    }

    public void ShowPrompt(string prompt)
    {
        if (_promptText)
        {
            _promptText.enabled = true;
        }
        if (_textBackground)
        {
            _textBackground.enabled = true;
        }
        _promptText.text = prompt;
        _promptText.color = Color.white;
        if (_resizeToFit)
        {
            _resizeToFit.FitText();
        }
       
    }
    /// <summary>
    ///Removes current prompt 
    /// </summary>
    public void RemovePrompt()
    {
        if (_promptText)
        {
            _promptText.enabled = false;
        }
        if (_textBackground)
        {
            _textBackground.enabled = false;
        }
    }
    /// <summary>
    ///Removes prompt if it contains input text
    /// </summary>

    public void RemovePrompt(string prompt)
    {
        if (_promptText)
        {
            if(_promptText.text != prompt)
            {
                return;
            }
            _promptText.enabled = false;
        }
        if (_textBackground)
        {
            _textBackground.enabled = false;
        }
    }
}
