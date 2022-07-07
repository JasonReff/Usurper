using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _startButtons;
    [SerializeField] private List<CharacterSelectPortrait> _portraits = new List<CharacterSelectPortrait>();

    private void OnEnable()
    {
        CharacterSelectDeck.OnDeckClicked += EquipDeck;
        CharacterSelectPortrait.OnDeckEquipped += SetStartButton;
        CharacterSelectPortrait.OnDeckUnequipped += SetStartButton;
    }

    private void OnDisable()
    {
        CharacterSelectDeck.OnDeckClicked -= EquipDeck;
        CharacterSelectPortrait.OnDeckEquipped -= SetStartButton;
        CharacterSelectPortrait.OnDeckUnequipped -= SetStartButton;
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

    private void EquipDeck(StartingDeck startingDeck)
    {
        var portrait = GetFirstUnselectedPortrait();
        if (portrait != null)
            portrait.UpdateDeckAndKing(startingDeck);
    }

    private CharacterSelectPortrait GetFirstUnselectedPortrait()
    {
        for (int i = 0; i < _portraits.Count; i++)
        {
            if (_portraits[i].EquippedDeck == null)
                return _portraits[i];
        }
        return null;
    }
}
