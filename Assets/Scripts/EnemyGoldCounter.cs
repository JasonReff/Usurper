using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyGoldCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _goldCounter;
    [SerializeField] private EnemyShopManager _shop;
    private void  Update()
    {
        _goldCounter.text = $"Enemy Gold: {_shop.Money}";
    }
}
