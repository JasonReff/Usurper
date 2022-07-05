using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class TutorialManager : MonoBehaviour
{
    [SerializeField] protected GameObject _tutorialCompletePanel, _backgroundCover;

    public abstract void StartTutorial();

    protected void TutorialComplete()
    {
        StartCoroutine(TutorialCompleteCoroutine());

        IEnumerator TutorialCompleteCoroutine()
        {
            yield return new WaitForSeconds(1f);
            _tutorialCompletePanel.SetActive(true);
            _backgroundCover.SetActive(true);
        }
        
    }
}
