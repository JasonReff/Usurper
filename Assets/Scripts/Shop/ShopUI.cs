using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private GameObject _shopParent;
    [SerializeField] protected Transform _cardParent;
    [SerializeField] protected List<Vector2> _cardPositions;
    [SerializeField] private TextMeshProUGUI _moneyText, _drawPileCount, _discardPileCount;
    [SerializeField] private ShopManager _manager;
    [SerializeField] protected PurchaseableUnit _cardPrefab;
    protected readonly List<PurchaseableUnit> _cardUIs = new List<PurchaseableUnit>();
    private bool _isShopTurn;

    public static Action OnCantAffordPurchase;

    public ShopManager Manager { get => _manager; set => _manager = value; }

    private void OnEnable()
    {
        BoardVisualizer.OnBoardCreated += HideShopDuringReview;
        BoardVisualizer.OnBoardHidden += ShowShopAfterReview;
    }

    private void OnDisable()
    {
        BoardVisualizer.OnBoardCreated -= HideShopDuringReview;
        BoardVisualizer.OnBoardHidden -= ShowShopAfterReview;
    }

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

    public virtual void ShowCards(List<UnitData> units, UnitFaction faction)
    {
        for (int i = 0; i < 3; i++)
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

    public void HideUnaffordableUnits()
    {
        foreach (var card in _cardUIs)
        {
            if (_manager.Money < card.Cost)
                card.HidePurchase();
        }
    }

    public void UpdatePileCounts(PlayerDeck deck)
    {
        _drawPileCount.text = deck.DrawCount.ToString();
        _discardPileCount.text = deck.DiscardCount.ToString();
    }

    private void HideShopDuringReview()
    {
        if (_shopParent.activeInHierarchy)
            _isShopTurn = true;
        else _isShopTurn = false;
        _shopParent.SetActive(false);
    }

    private void ShowShopAfterReview()
    {
        if (_isShopTurn)
            _shopParent.SetActive(true);
    }

    public void SkipPurchase()
    {
        _manager.SkipShopPhase();
    }
}
