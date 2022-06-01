using UnityEngine.EventSystems;
using Photon.Pun;
using UnityEngine;

public class OnlineBoardTile : BoardTile
{
    private void Awake()
    {
        
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        OnlineBoard board = Board.Instance as OnlineBoard;
        board.SelectTile(TilePosition());
    }

    public override void PlaceUnit(UnitData unitData, UnitFaction faction)
    {
        GameObject unitObject = PhotonNetwork.Instantiate(unitData.UnitName, transform.position, Quaternion.identity, 0, new object[] { faction });
        var unit = unitObject.GetComponent<Unit>();
        unit.Faction = faction;
        _unit = unit;
        unit.Tile = this;
        unit.SetSprite(unitData.GetSprite(faction));
        OnUnitPlaced?.Invoke(unit);
    }
}
