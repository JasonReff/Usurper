using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private GameObject _shopParent;
    [SerializeField] private Image _coinPrefab;
    [SerializeField] protected Transform _cardParent, _coinParent;
    [SerializeField] protected List<Vector2> _cardPositions;
    [SerializeField] private TextMeshProUGUI _moneyText, _drawPileCount, _discardPileCount;
    [SerializeField] private ShopManager _manager;
    [SerializeField] protected PurchaseableUnit _cardPrefab;
    protected readonly List<PurchaseableUnit> _cardUIs = new List<PurchaseableUnit>();
    private bool _isShopTurn;
    private bool _isDisabled;

    public static Action OnCantAffordPurchase;
    public static Action OnCoinGained;

    public ShopManager Manager { get => _manager; set => _manager = value; }
    public GameObject ShopParent { get => _shopParent; }

    private void OnEnable()
    {
        BoardVisualizer.OnBoardCreated += HideShopDuringReview;
        BoardVisualizer.OnBoardHidden += ShowShopAfterReview;
    }

    private void OnDisable()
    {
        BoardVisualizer.OnBoardCreated -= HideShopDuringReview;
        BoardVisualizer.OnBoardHidden -= ShowShopAfterReview;
        _isDisabled = true;
    }

    public void ShowShop()
    {
        _shopParent.SetActive(true);
        UpdateMoney();
    }

    public void HideShop()
    {
        HideCards();
        _shopParent.SetActive(false);
    }

    public void UpdateMoney()
    {
        _moneyText.text = $"Money: {_manager.Money}";
        CoinEffect();
        UpdatePurchaseButtons();
    }

    public virtual void ShowCards(List<UnitCard> units, UnitFaction faction)
    {
        for (int i = 0; i < 3; i++)
        {
            if (i >= units.Count)
                return;
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
            if (!gameObject.activeInHierarchy)
                continue;
            if (!_cardUIs[i].gameObject.activeInHierarchy)
                continue;
            Destroy(_cardUIs[i].gameObject);
            _cardUIs.RemoveAt(i);
        }
    }

    public void UpdatePurchaseButtons()
    {
        foreach (var card in _cardUIs)
        {
            if (_manager.Money < card.Cost)
                card.HidePurchase();
            else
                card.ShowPurchase();
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

    public void CoinEffect()
    {
        OnCoinGained?.Invoke();
        var coin = Instantiate(_coinPrefab, _coinParent);
        var up = (Vector2)coin.transform.position + Vector2.up;
        coin.transform.DOMove(up, 1f).OnComplete(() => 
        {
            coin.DOFade(0, 0.25f).OnComplete(() => 
            {
                Destroy(coin.gameObject);
            });
        });
    }
}
