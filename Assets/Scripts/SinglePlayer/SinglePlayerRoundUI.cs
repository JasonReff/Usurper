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
        if (_stats.Round == 0)
            _roundText.text = "Practice";
        else
            _roundText.text = $"Round: {_stats.Round}";
    }
}
