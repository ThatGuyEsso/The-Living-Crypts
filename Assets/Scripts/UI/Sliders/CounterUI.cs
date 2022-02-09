using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countDisplay;

    [SerializeField] private TextMeshProUGUI countLabel;
    public void UpdateDisplay(int count) {
        countDisplay.text = count.ToString();
    }

    public void UpdateLabel(string label)
    {
        countLabel.text = label;
    }
}
