using Photon.Pun;
using System;
using UnityEngine;

public class OnlinePlayerManager : PlayerManager, IPunInstantiateMagicCallback
{
    [SerializeField] private PlayerDeck _onlineDeck;
    public static Action<UnitFaction, string> OnFactionAssigned;
    public static Action<UnitFaction> OnPlayerOneFactionAssigned;
    public override void AddCharacterEvents(GameState state)
    {
        if (IsGameStateCorrectFaction(state) && state.GetType() == typeof(MoveUnitState))
            OnlineBoard.OnTileSelected += SelectTile;
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (!photonView.IsMine || PhotonNetwork.LocalPlayer.ActorNumber != 1)
            return;
        UnitFaction faction = (UnitFaction)UnityEngine.Random.Range(0, 2);
        AssignPlayerFaction(faction);
        GetComponent<OnlineKingPlacer>().PlaceKing();
        this.photonView.RPC("OnFactionAssignedRPC", RpcTarget.All, new object[] { faction });
    }

    public override void RemoveCharacterEvents(GameState state)
    {
        if (!IsGameStateCorrectFaction(state) || state.GetType() != typeof(MoveUnitState))
            OnlineBoard.OnTileSelected -= SelectTile;
    }

    public override void SelectTile(BoardTile tile)
    {
        if (!this.photonView.IsMine)
            return;
        base.SelectTile(tile);
    }

    private void AssignPlayerFaction(UnitFaction faction)
    {
        _faction = faction;
        _onlineDeck.Faction = _faction;
        OnFactionAssigned?.Invoke(_faction, PhotonNetwork.NickName);
    }

    [PunRPC]
    private void OnFactionAssignedRPC(UnitFaction faction)
    {
        OnPlayerOneFactionAssigned?.Invoke(faction);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        OnPlayerOneFactionAssigned += SetPlayerTwoFaction;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        OnPlayerOneFactionAssigned -= SetPlayerTwoFaction;
    }

    private void SetPlayerTwoFaction(UnitFaction playerOneFaction)
    {
        if (photonView.IsMine && PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            AssignPlayerFaction(playerOneFaction.GetOpposite());
            GetComponent<OnlineKingPlacer>().PlaceKing();
        }
    }
}