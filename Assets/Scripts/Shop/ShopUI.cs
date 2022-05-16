using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private GameObject _shopParent;
    [SerializeField] private Transform _cardParent;
    [SerializeField] private List<Vector2> _cardPositions;
    [SerializeField] private TextMeshProUGUI _moneyText, _drawPileCount, _discardPileCount;
    [SerializeField] private ShopManager _manager;
    [SerializeField] private PurchaseableUnit _cardPrefab;
    private readonly List<PurchaseableUnit> _cardUIs = new List<PurchaseableUnit>();

    public void ShowShop()
    {
        _shopParent.SetActive(true);
        _moneyText.text = $"Money: {_manager.Money}";
    }

    public void HideShop()
    {
        HideCards();
        _shopParent.SetActive(false);
    }

    public void ShowCards(List<UnitData> units, UnitFaction faction)
    {
        for (int i = 0; i < units.Count; i++)
        {
            var unit = units[i];
            var card = Instantiate(_cardPrefab, _cardParent);
            card.transform.localPosition = _cardPositions[i];
            card.LoadUnitData(unit, faction);
            _cardUIs.Add(card);
        }
    }

    public void HideCards()
    {
        for (int i = _cardUIs.Count - 1; i >= 0; i--)
        {
            Destroy(_cardUIs[i].gameObject);
            _cardUIs.RemoveAt(i);
        }
    }

    public void UpdatePileCounts(PlayerDeck deck)
    {
        _drawPileCount.text = deck.DrawCount.ToString();
        _discardPileCount.text = deck.DiscardCount.ToString();
    }
}