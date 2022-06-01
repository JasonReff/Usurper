using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SpawnPlayers : MonoBehaviourPunCallbacks
{
    [SerializeField] private OnlineBoard _board;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _shopPrefab;
    [SerializeField] private GameObject _shopUIPrefab;
    [SerializeField] private ShopUI _playerUI, _enemyUI;
    [SerializeField] private Transform _canvas;
    private OnlineShopManager _shop;

    private void Start()
    {
        FindBoard();
        var playerManager = PhotonNetwork.Instantiate(_playerPrefab.name, Vector3.zero, Quaternion.identity);
        SetShopAndUI();
        _shop.SetFaction(playerManager.GetComponent<OnlinePlayerManager>().Faction);
        _shop.SetKing(playerManager.GetComponent<OnlineKingPlacer>().King);
    }

    private void FindBoard()
    {
        Board.Instance = _board;
    }

    private void SetShopAndUI()
    {
        var shop = PhotonNetwork.Instantiate(_shopPrefab.name, Vector3.zero, Quaternion.identity);
        _shop = shop.GetComponent<OnlineShopManager>();
        if (_shop.GetFaction() == UnitFaction.Player)
        {
            _shop.SetUI(_playerUI);
            _playerUI.Manager = _shop;
        }
        else
        {
            _shop.SetUI(_enemyUI);
            _enemyUI.Manager = _shop;
        }
    }
}
