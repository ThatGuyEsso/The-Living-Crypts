using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemDisplayManager : MonoBehaviour
{
    [SerializeField] private Image PrimaryIcon;
    [SerializeField] private Image SecondaryIcon;


    public void SetPrimaryIcon(Sprite newIcon)
    {
        if (newIcon)
        {
            PrimaryIcon.gameObject.SetActive(true);
            PrimaryIcon.sprite = newIcon;
        }
        else
        {
            PrimaryIcon.gameObject.SetActive(false);
        }
    }
    public void SetSecondary(Sprite newIcon)
    {
        if (newIcon)
        {
            SecondaryIcon.gameObject.SetActive(true);
            SecondaryIcon.sprite = newIcon;
        }
        else
        {
            SecondaryIcon.gameObject.SetActive(false);
        }
    }


}
