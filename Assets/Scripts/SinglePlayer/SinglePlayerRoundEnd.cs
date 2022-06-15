using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlayerRoundEnd : MonoBehaviour
{
    [SerializeField] private List<CardReward> _rewards;
    [SerializeField] private RemovableCard _removableCardPrefab;
    [SerializeField] private Transform _removableCardParent;
    [SerializeField] private CardPool _cardPool;
    private UnitData _selectedCard = null;
    [SerializeField] private PlayerDeck _playerDeck;
    [SerializeField] private GameObject _confirmButton;
    [SerializeField] private EnemyDeckGenerator _enemyDeck;
    [SerializeField] private SinglePlayerStats _stats;
    [SerializeField] private SceneLoader _sceneLoader;
    private bool _cardAdded;
    private bool _cardRemoved;

    private void OnEnable()
    {
        CardReward.OnCardSelected += SelectUnit;
        RemovableCard.OnCardSelected += SelectUnitToRemove;
    }

    private void OnDisable()
    {
        CardReward.OnCardSelected -= SelectUnit;
        RemovableCard.OnCardSelected -= SelectUnitToRemove;
    }

    private void Awake()
    {
        SetRewardCards();
        SetRemovableCards();
    }

    private void SetRewardCards()
    {
        System.Random random = new System.Random();
        List<UnitData> units = _cardPool.UnitPool.Pull(3);
        for (int i = 0; i < _rewards.Count; i++)
        {
            var reward = _rewards[i];
            reward.SetUnit(units[i]);
        }
    }

    private void SetRemovableCards()
    {
        foreach (var unit in _playerDeck.AllCards())
        {
            var removable = Instantiate(_removableCardPrefab, _removableCardParent);
            removable.SetUnit(unit);
        }
    }

    private void SelectUnit(UnitData unitData)
    {
        _selectedCard = unitData;
        _confirmButton.SetActive(true);
        _cardAdded = true;
        _cardRemoved = false;
    }

    private void SelectUnitToRemove(UnitData unitData)
    {
        _selectedCard = unitData;
        _confirmButton.SetActive(true);
        _cardAdded = false;
        _cardRemoved = true;
    }

    public void Confirm()
    {
        if (_selectedCard == null)
            return;
        if (_cardAdded)
        {
            _playerDeck.AddCard(_selectedCard);
            GoToNextRound();
        }
        else if (_cardRemoved)
        {
            _playerDeck.RemoveCard(_selectedCard);
            GoToNextRound();
        }  
    }

    private void GoToNextRound()
    {
        if (_stats.Round < 10)
        {
            _stats.Round++;
            _enemyDeck.SetDeck(_stats.Round);
            _sceneLoader.LoadScene("SinglePlayerGame");
        }
    }
}
