using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WeaponDisplayManager : MonoBehaviour
{
    [SerializeField] private Image PrimaryWeaponImage;
    [SerializeField] private CircularProgressBar PrimaryWeaponCooldownDisplay;
    [SerializeField] private UIElementShake PrimaryAttackShake;
    [SerializeField] private Image SecondaryWeaponImage;
    [SerializeField] private CircularProgressBar SecondaryWeaponCooldownDisplay;
    [SerializeField] private UIElementShake SecondaryElementShake;

    public void SetUp(Sprite primaryImage, Sprite secondaryImage,float primaryMaxCooldown, float secondaryMaxCooldown)
    {
        if (primaryImage)
        {
            PrimaryWeaponImage.sprite = primaryImage;
            PrimaryWeaponImage.gameObject.SetActive(true);
        }

        PrimaryWeaponCooldownDisplay.SetMaxValue(primaryMaxCooldown);
        PrimaryWeaponCooldownDisplay.UpdateValue(0f);

        if (secondaryImage)
        {
            SecondaryWeaponImage.sprite = secondaryImage;
            SecondaryWeaponImage.gameObject.SetActive(true);
        }

        SecondaryWeaponCooldownDisplay.SetMaxValue(secondaryMaxCooldown);
        SecondaryWeaponCooldownDisplay.UpdateValue(0f);
    }


    public void UpdatePrimaryCooldown(float newValue)
    {
        PrimaryWeaponCooldownDisplay.UpdateValue(newValue);
        if(newValue <=0 && PrimaryAttackShake)
        {
            PrimaryAttackShake.BeginViewBob();
        }
    }


    public void UpdateSecondaryCooldown(float newValue)
    {
        SecondaryWeaponCooldownDisplay.UpdateValue(newValue);
        if (newValue <= 0 && SecondaryElementShake)
        {
            SecondaryElementShake.BeginViewBob();
        }
    }
}
