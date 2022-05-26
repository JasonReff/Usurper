using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private float _minX, _maxX, _minY, _maxY;

    private void Start()
    {
        Vector2 randomPosition = new Vector2(UnityEngine.Random.Range(_minX, _maxX), UnityEngine.Random.Range(_minY, _maxY));
        PhotonNetwork.Instantiate(_playerPrefab.name, randomPosition, Quaternion.identity);
    }
}
