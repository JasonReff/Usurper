using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _startButtons;
    [SerializeField] private List<CharacterSelectPortrait> _portraits = new List<CharacterSelectPortrait>();

    private void OnEnable()
    {
        CharacterSelectPortrait.OnDeckEquipped += SetStartButton;
    }

    private void OnDisable()
    {
        CharacterSelectPortrait.OnDeckEquipped -= SetStartButton;
    }

    private bool CheckPortraits()
    {
        foreach (var portrait in _portraits)
        {
            if (portrait.EquippedDeck == null)
                return false;
        }
        return true;
    }

    private void SetStartButton()
    {
        if (CheckPortraits())
            ActivateButtons(true);
        else ActivateButtons(false);
    }

    private void ActivateButtons(bool isActive)
    {
        foreach (var button in _startButtons)
            button.SetActive(isActive);
    }
}
