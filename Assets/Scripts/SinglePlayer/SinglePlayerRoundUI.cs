using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SinglePlayerRoundUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _roundText;
    [SerializeField] private SinglePlayerStats _stats;
    private void Awake()
    {
        _roundText.text = $"Round: {_stats.Round}";
    }
}
