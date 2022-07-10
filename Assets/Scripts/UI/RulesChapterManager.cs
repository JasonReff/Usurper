using System.Collections.Generic;
using UnityEngine;

public class RulesChapterManager : MonoBehaviour
{
    [SerializeField] private List<RulesChapter> _chapters = new List<RulesChapter>();

    public void SetChapterHighlight(int pageNumber)
    {
        foreach (var chapter in _chapters)
            chapter.SetHighlight(pageNumber);
    }
}
