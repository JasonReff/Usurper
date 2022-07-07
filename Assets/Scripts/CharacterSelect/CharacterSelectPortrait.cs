using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSelectPortrait : MonoBehaviour
{
    [SerializeField] private Image _kingIcon;
    [SerializeField] private UnitFaction _faction;
    [SerializeField] private PlayerDeck _playerDeck;
    [SerializeField] private TextMeshProUGUI _deckName;
    [SerializeField] private StartingDeck _equippedDeck;
    [SerializeField] private Vector2 _easePosition;
    [SerializeField] private GameObject _unequipButton;
    private Vector2 _startingPosition;

    public StartingDeck EquippedDeck { get => _equippedDeck; set => _equippedDeck = value; }

    public static Action OnDeckEquipped;
    public static Action OnDeckUnequipped;

    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    CharacterSelectDeck.OnDeckDropped += UpdateDeckAndKing;
    //}

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    CharacterSelectDeck.OnDeckDropped -= UpdateDeckAndKing;
    //}

    private void Start()
    {
        _startingPosition = _kingIcon.transform.localPosition;
    }

    public void UpdateDeckAndKing(StartingDeck deck)
    {
        _equippedDeck = deck;
        if (_faction == UnitFaction.Player)
            _kingIcon.sprite = deck.King.PlayerSprite;
        else
            _kingIcon.sprite = deck.King.EnemySprite;
        KingAnimation();
        _playerDeck.ResetDeck(deck);
        _deckName.text = deck.DeckName;
        _unequipButton.SetActive(true);
        OnDeckEquipped?.Invoke();

        void KingAnimation()
        {
            _kingIcon.transform.localPosition = _startingPosition;
            _kingIcon.DOFade(0f, 0f);
            _kingIcon.DOFade(1f, 0.5f);
            _kingIcon.transform.DOLocalMoveX(_easePosition.x, 0.5f).SetEase(Ease.OutSine);
        }
    }

    public void UnequipDeck()
    {
        _equippedDeck = null;
        _deckName.text = "";
        _kingIcon.DOFade(0f, 0.5f);
        _kingIcon.transform.DOLocalMoveX(_startingPosition.x, 0.5f).SetEase(Ease.OutSine);
        _unequipButton.SetActive(false);
        OnDeckUnequipped?.Invoke();
    }
}
