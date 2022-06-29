using System;
using TMPro;
using UnityEngine;

public class RemovableCard : MonoBehaviour
{
    private UnitData _unitData;
    [SerializeField] private TextMeshProUGUI _unitName;
    [SerializeField] private GameObject _highlight;
    public static Action<RemovableCard> OnCardSelected;

    public UnitData UnitData { get => _unitData; }

    private void OnEnable()
    {
        OnCardSelected += SetHighlight;
        CardReward.OnCardSelected += RemoveHighlight;
    }

    private void OnDisable()
    {
        OnCardSelected -= SetHighlight;
        CardReward.OnCardSelected -= RemoveHighlight;
    }
    public void SetUnit(UnitData unitData)
    {
        _unitData = unitData;
        _unitName.text = _unitData.UnitName;
    }

    public void SelectUnit()
    {
        OnCardSelected?.Invoke(this);
    }

    private void SetHighlight(RemovableCard removableCard)
    {
        if (removableCard != this)
            _highlight.SetActive(false);
        else _highlight.SetActive(true);
    }

    private void RemoveHighlight(CardReward cardReward)
    {
        _highlight.SetActive(false);
    }
}