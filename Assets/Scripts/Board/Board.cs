using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour, IBoard<BoardTile>
{
    public static Board Instance;

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private BoardTile[] tileArray;

    public BoardTile[] TileArray { get => tileArray; set => tileArray = value; }

    public List<Unit> PlayerUnits = new List<Unit>(), EnemyUnits = new List<Unit>();

    private void OnEnable()
    {
        BoardTile.OnUnitPlaced += AddUnitToList;
        Unit.OnUnitDeath += RemoveUnitFromList;
    }

    private void OnDisable()
    {
        BoardTile.OnUnitPlaced -= AddUnitToList;
        Unit.OnUnitDeath -= RemoveUnitFromList;
    }

    public BoardTile? GetTileAtPosition(Vector2 position)
    {
        if (Instance.tileArray.Where(t => (Vector2)t.transform.localPosition == position).Count() == 1)
            return Instance.tileArray.First(t => (Vector2)t.transform.localPosition == position);
        else return null;
    }

    private void AddUnitToList(Unit unit)
    {
        if (unit.Faction == UnitFaction.Player)
            PlayerUnits.Add(unit);
        else EnemyUnits.Add(unit);
    }

    private void RemoveUnitFromList(Unit unit)
    {
        if (unit.Faction == UnitFaction.Player)
            PlayerUnits.Remove(unit);
        else EnemyUnits.Remove(unit);
    }
}

public interface IBoard<T> where T : IBoardTile
{
    public T[] TileArray { get; }

    public T GetTileAtPosition(Vector2 position);
}
