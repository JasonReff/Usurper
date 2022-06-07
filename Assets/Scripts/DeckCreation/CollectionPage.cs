using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CollectionPage : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private List<UnitData> _units = new List<UnitData>();
    [SerializeField] private CollectionCard _cardPrefab;
    [SerializeField] private Transform _collectionPanel;
    [SerializeField] private bool _startingPage;
    private List<CollectionCard> _cards = new List<CollectionCard>();
    public static Action<CollectionPage> OnPageClicked;

    private void OnEnable()
    {
        OnPageClicked += ClearPage;
        OnPageClicked += FillPage;
    }

    private void OnDisable()
    {
        OnPageClicked -= ClearPage;
        OnPageClicked -= FillPage;
    }

    private void Start()
    {
        if (_startingPage)
        {
            Fill();
        }
    }

    private void ClearPage(CollectionPage page)
    {
        if (page != this)
        {
            Clear();
        }
    }

    private void Clear()
    {
        for (int i = _cards.Count - 1; i >= 0; i--)
        {
            var card = _cards[i];
            _cards.RemoveAt(i);
            Destroy(card.gameObject);
        }
    }

    private void Fill()
    {
        for (int i = 0; i < _units.Count; i++)
        {
            var card = Instantiate(_cardPrefab, _collectionPanel);
            card.SetCard(_units[i]);
            _cards.Add(card);
        }
    }

    private void FillPage(CollectionPage page)
    {
        if (page == this)
        {
            Clear();
            Fill();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnPageClicked?.Invoke(this);
    }
}
