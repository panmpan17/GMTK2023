using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tutorial : MonoBehaviour, IPointerClickHandler
{
    public static bool IsSkip = false;

    [SerializeField]
    private GameObject[] pages;

    private int _currentPageIndex = 0;

    void Start()
    {
        pages[_currentPageIndex].SetActive(true);

        for (int i = 1; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
        }

        GetComponent<Canvas>().enabled = !IsSkip;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        pages[_currentPageIndex].SetActive(false);
        _currentPageIndex++;

        if (_currentPageIndex >= pages.Length)
        {
            gameObject.SetActive(false);
            return;
        }
        pages[_currentPageIndex].SetActive(true);
    }
}
