using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class CardPileUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private ShopManager _shop;
    [SerializeField] protected PlayerDeck _deck;
    [SerializeField] private Image _panel;
    [SerializeField] private TextMeshProUGUI _cardText;

    private void Awake()
    {
        _deck = _shop.Deck;
    }

    private void ShowCards()
    {
        _panel.enabled = true;
        _cardText.text = BuildCardString();
    }

    private void HideCards()
    {
        _panel.enabled = false;
        _cardText.text = null;
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

    protected abstract List<UnitData> GetCards();

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowCards();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideCards();
    }
}
