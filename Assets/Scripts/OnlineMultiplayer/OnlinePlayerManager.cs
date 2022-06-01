using Photon.Pun;
using UnityEngine;

public class OnlinePlayerManager : PlayerManager, IPunInstantiateMagicCallback
{
    [SerializeField] private PlayerDeck _onlineDeck;
    public override void AddCharacterEvents(GameState state)
    {
        if (IsGameStateCorrectFaction(state) && state.GetType() == typeof(MoveUnitState))
            OnlineBoard.OnTileSelected += SelectTile;
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (!photonView.IsMine)
            return;
        AssignPlayerFaction();
        GetComponent<OnlineKingPlacer>().PlaceKing();
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

    private void AssignPlayerFaction()
    {
        if (photonView.IsMine && PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            _faction = UnitFaction.Player;
            _onlineDeck.Faction = _faction;
        }
        else if (photonView.IsMine && PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            _faction = UnitFaction.Enemy;
            _onlineDeck.Faction = _faction;
        }
    }
}