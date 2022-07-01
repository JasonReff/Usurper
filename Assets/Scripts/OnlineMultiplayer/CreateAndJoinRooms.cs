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
    [SerializeField] private TMP_InputField _roomID;
    [SerializeField] private PrivateMatchSettings _privateMatchSettings;
    private string _roomName;

    public void UpdateRoomName()
    {
        _roomName = _roomID.text;
    }

    public void SearchForRoom()
    {
        _privateMatchSettings.IsPrivateMatch = true;
        PhotonNetwork.JoinOrCreateRoom(_roomName, new RoomOptions() { MaxPlayers = 2, IsVisible = false}, PhotonNetwork.CurrentLobby);
    }

    private void JoinRoom()
    {
        _privateMatchSettings.IsPrivateMatch = false;
        PhotonNetwork.JoinRandomRoom();
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
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
        if (PhotonNetwork.CurrentRoom != null)
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

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
    }
}
