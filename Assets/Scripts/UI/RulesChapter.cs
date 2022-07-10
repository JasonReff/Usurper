using UnityEngine;
using UnityEngine.UI;

public class RulesChapter : MonoBehaviour
{
    [SerializeField] private int _firstPage, _lastPage;
    [SerializeField] private Image _highlight;

    public void SetHighlight(int pageNumber)
    {
        if (pageNumber < _firstPage || pageNumber > _lastPage)
        {
            _highlight.enabled = false;
        }
        else _highlight.enabled = true;
    }
}