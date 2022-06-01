using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class OnlineShopUI : ShopUI
{
    public override void ShowCards(List<UnitData> units, UnitFaction faction)
    {
        for (int i = 0; i < 3; i++)
        {
            var unit = units[i];
            var card = PhotonNetwork.Instantiate(_cardPrefab.name, Vector3.zero, Quaternion.identity);
            card.transform.parent = _cardParent;
            card.transform.localPosition = _cardPositions[i];
            card.GetComponent<PurchaseableUnit>().LoadUnitData(unit, faction);
            _cardUIs.Add(card.GetComponent<PurchaseableUnit>());
        }
    }
}