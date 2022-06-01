using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI _searchingText;

    private void JoinRoom()
    {
        PhotonNetwork.JoinRoom("a");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom("a");
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            PhotonNetwork.LoadLevel("OnlineMultiplayerGame");
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            PhotonNetwork.LoadLevel("OnlineMultiplayerGame");
        }
    }

    public void BeginSearch()
    {
        StartCoroutine(BeginSearchCoroutine());
        JoinRoom();
    }

    public void CancelSearch()
    {
        StopCoroutine(BeginSearchCoroutine());
        PhotonNetwork.LeaveRoom();
    }

    private IEnumerator BeginSearchCoroutine()
    {
        _searchingText.text = "Searching.";
        yield return new WaitForSeconds(1);
        _searchingText.text = "Searching..";
        yield return new WaitForSeconds(1);
        _searchingText.text = "Searching...";
        yield return new WaitForSeconds(1);
        StartCoroutine(BeginSearchCoroutine());
    }

    public void LeaveServer()
    {
        PhotonNetwork.Disconnect();
    }
}
