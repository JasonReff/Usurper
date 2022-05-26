using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardReward : MonoBehaviour
{
    [SerializeField] private UnitData _unitData;
    [SerializeField] private Image _unitImage, _moveset;
    [SerializeField] private TextMeshProUGUI _cost;
    public static Action<UnitData> OnCardSelected; 
    public void SelectUnit()
    {
        OnCardSelected?.Invoke(_unitData);
    }

    public void SetUnit(UnitData unitData)
    {
        _unitData = unitData;
        _unitImage.sprite = _unitData.PlayerSprite;
        _moveset.sprite = _unitData.Moveset;
        _cost.text = _unitData.Cost.ToString();
    }
}
