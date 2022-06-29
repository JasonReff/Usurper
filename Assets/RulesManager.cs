using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RulesManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _pages = new List<GameObject>();
    [SerializeField] private TextMeshProUGUI _pageNumberTextbox;
    [SerializeField] private GameObject _currentPage;
    [SerializeField] private int _pageNumber = 1;

    private void Awake()
    {
        SetPageNumberText();
    }
    public void GoToPage(int pageNumber)
    {
        if (_pages.Count >= pageNumber && pageNumber >= 1)
        {
            _currentPage?.SetActive(false);
            _currentPage = _pages[pageNumber - 1];
            _currentPage.SetActive(true);
            _pageNumber = pageNumber;
            SetPageNumberText();
        }
    }

    private void SetPageNumberText()
    {
        _pageNumberTextbox.text = $"{_pageNumber} / {_pages.Count}";
    }

    public void NextPage()
    {
        GoToPage(_pageNumber + 1);
    }

    public void PreviousPage()
    {
        GoToPage(_pageNumber - 1);
    }
}
