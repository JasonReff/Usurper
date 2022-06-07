using UnityEngine.EventSystems;
using Photon.Pun;
using UnityEngine;

public class OnlineBoardTile : BoardTile
{

    public override void OnEnable()
    {
        base.OnEnable();
        Unit.OnUnitDeath += ClearHoveredPiece;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        Unit.OnUnitDeath -= ClearHoveredPiece;
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        OnlineBoard board = Board.Instance as OnlineBoard;
        board.SelectTile(TilePosition());
    }

    public override void PlaceUnit(UnitData unitData, UnitFaction faction)
    {
        GameObject unitObject = PhotonNetwork.Instantiate(unitData.name, transform.position, Quaternion.identity, 0, new object[] { faction });
        var unit = unitObject.GetComponent<Unit>();
        unit.Faction = faction;
        _unit = unit;
        unit.Tile = this;
        unit.SetSprite(unitData.GetSprite(faction));
        OnUnitPlaced?.Invoke(unit);
        if (TryGetComponent<OnlineUnitCallbacks>(out var onlineUnit))
        {
            onlineUnit.photonView.RPC("OnUnitPlacedCallback", RpcTarget.Others);
        }
    }

    private void ClearHoveredPiece(Unit unit)
    {
        if (unit == _unit)
        {
            OnMouseExit?.Invoke();
        }
    }
}
