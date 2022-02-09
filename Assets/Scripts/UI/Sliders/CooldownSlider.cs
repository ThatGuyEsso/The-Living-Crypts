using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CooldownSlider : MonoBehaviour
{
    private Transform targetTransfrom;
    [SerializeField] private Vector2 targetoffset;

    [SerializeField] private Image sliderFill;
    //[SerializeField] private Image sliderBackground;

    private bool trackTarget;
    private void LateUpdate()
    {
        if (targetTransfrom&&trackTarget)
        {
            transform.position = targetTransfrom.position + (Vector3)targetoffset;
        }
    }


    public void SetUpCooldownSlider(Color sliderColour, Transform target,bool isFollowingTarget,Vector2 offset)
    {
        sliderFill.color = sliderColour;
        //sliderBackground.color = sliderColour;

        targetTransfrom = target;

        trackTarget = isFollowingTarget;
        targetoffset = offset;
    }

    public void SetOffSet(Vector2 newOffset) { targetoffset = newOffset; }
}
