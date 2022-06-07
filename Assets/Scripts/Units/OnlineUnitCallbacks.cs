using Photon.Pun;
using UnityEngine;

public class OnlineUnitCallbacks : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        UnitFaction faction = (UnitFaction)info.photonView.InstantiationData[0];
        var tile = Board.Instance.GetTileAtPosition(transform.localPosition);
        var unit = GetComponent<Unit>();
        tile.UnitOnTile = unit;
        unit.Tile = tile;
        unit.Faction = faction;
        unit.SetSprite(unit.UnitData.GetSprite(faction));
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
    }

    [PunRPC]
    private void OnUnitPlacedCallback()
    {
        Board.Instance.AddUnitToList(GetComponent<Unit>());
    }

}
