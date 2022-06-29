using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class SinglePlayerRunEnd : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _deckList;
    [SerializeField] private PlayerDeck _deck;
    [SerializeField] private PlayerStats _stats;

    public void SetString()
    {
        _deckList.text = BuildCardString();
        _stats.SinglePlayerRunsWon++;
    }

    private string BuildCardString()
    {
        var cards = GetCards();
        if (cards.Count == 0)
            return "(Empty)";
        StringBuilder cardString = new StringBuilder();
        cardString.Append(cards[0].UnitName);
        for (int i = 1; i < cards.Count; i++)
        {
            cardString.Append("\n" + cards[i].UnitName);
        }
        return cardString.ToString();
    }

    private List<UnitData> GetCards()
    {
        _deck.ShuffleHandAndDiscardIntoDrawPile();
        return _deck.DrawPile;
    }
}
