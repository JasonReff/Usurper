using Photon.Pun;
using System;
using UnityEngine;

public class OnlineUnitCallbacks : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    public static Action<Unit> OnlineUnitPlaced;
    public static Action OnlineUnitMoved;
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        UnitFaction faction = (UnitFaction)info.photonView.InstantiationData[0];
        var tile = Board.Instance.GetTileAtPosition(transform.localPosition);
        var unit = GetComponent<Unit>();
        tile.UnitOnTile = unit;
        unit.Tile = tile;
        unit.Faction = faction;
        unit.SetSprite(unit.UnitData.GetSprite(faction));
        OnlineUnitPlaced?.Invoke(unit);
    }

    public void OnUnitMoved(Vector2 oldPosition, Vector2 newPosition)
    {
        this.photonView.RPC("OnUnitMovedCallback", RpcTarget.Others, new object[] { oldPosition, newPosition});
    }

    [PunRPC]
    private void OnUnitMovedCallback(Vector2 oldPosition, Vector2 newPosition)
    {
        var unit = Board.Instance.GetTileAtPosition(oldPosition).UnitOnTile as Unit;
        if (unit == null)
            return;
        unit.ForceMoveToTile(Board.Instance.GetTileAtPosition(newPosition));
        OnlineUnitMoved?.Invoke();
    }

    [PunRPC]
    private void OnUnitPlacedCallback()
    {
        Board.Instance.AddUnitToList(GetComponent<Unit>());
    }

}
