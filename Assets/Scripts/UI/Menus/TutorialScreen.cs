using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScreen : MonoBehaviour
{
    [SerializeField] private GameObject[] Pages;
    [SerializeField] private GameObject NextButton;
    [SerializeField] private GameObject PreviousButton;

    [SerializeField] private int currentPageIndex = 0;

    private void Awake()
    {
        if (Pages.Length > 0)
        {
            if (Pages.Length >1)
            {
                for(int i =1; i< Pages.Length; i++)
                {
                    Pages[i].gameObject.SetActive(false);
                    PreviousButton.gameObject.SetActive(false);
                }
            }
        }
    }

    public void OnNextPage()
    {
        Pages[currentPageIndex].gameObject.SetActive(false);
        PreviousButton.gameObject.SetActive(true);
        currentPageIndex++;
        if(currentPageIndex>= Pages.Length-1)
        {
            NextButton.gameObject.SetActive(false);
        }

        Pages[currentPageIndex].gameObject.SetActive(true);
    }
    public void OnPreviousPage()
    {
        NextButton.gameObject.SetActive(true);
        Pages[currentPageIndex].gameObject.SetActive(false);
        currentPageIndex--;
        if (currentPageIndex <=0)
        {
            PreviousButton.gameObject.SetActive(false);
        }

        Pages[currentPageIndex].gameObject.SetActive(true);
    }
}
