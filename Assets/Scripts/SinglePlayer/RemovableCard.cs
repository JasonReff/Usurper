using System;
using TMPro;
using UnityEngine;

public class RemovableCard : MonoBehaviour
{
    private UnitData _unitData;
    [SerializeField] private TextMeshProUGUI _unitName;
    public static Action<UnitData> OnCardSelected;
    public void SetUnit(UnitData unitData)
    {
        _unitData = unitData;
        _unitName.text = _unitData.UnitName;
    }

    public void SelectUnit()
    {
        OnCardSelected?.Invoke(_unitData);
    }
}