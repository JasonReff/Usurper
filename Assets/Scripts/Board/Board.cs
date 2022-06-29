using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviourPunCallbacks, IBoard<BoardTile>
{
    public static Board Instance;
    [SerializeField] private bool _demo;

    private void Awake()
    {
        if (!_demo)
            Instance = this;
    }

    [SerializeField] private BoardTile[] tileArray;
    public static Action<Unit> OnUnitPlaced;
    public static Action<Unit> OnUnitRemoved;

    public BoardTile[] TileArray { get => tileArray; set => tileArray = value; }

    public List<Unit> PlayerUnits = new List<Unit>(), EnemyUnits = new List<Unit>();

    protected virtual void OnEnable()
    {
        BoardTile.OnUnitPlaced += AddUnitToList;
        Unit.OnUnitDeath += RemoveUnitFromList;
        KingUnit.OnKingCaptured += RemoveUnitFromList;
    }

    protected virtual void OnDisable()
    {
        BoardTile.OnUnitPlaced -= AddUnitToList;
        Unit.OnUnitDeath -= RemoveUnitFromList;
        KingUnit.OnKingCaptured -= RemoveUnitFromList;
    }

    public BoardTile? GetTileAtPosition(Vector2 position)
    {
        if (Instance.tileArray.Where(t => (Vector2)t.transform.localPosition == position).Count() == 1)
            return Instance.tileArray.First(t => (Vector2)t.transform.localPosition == position);
        else return null;
    }

    public void AddUnitToList(Unit unit)
    {
        if (_demo)
            return;
        if (PlayerUnits.Contains(unit) || EnemyUnits.Contains(unit))
            return;
        if (unit.Faction == UnitFaction.Player)
            PlayerUnits.Add(unit);
        else EnemyUnits.Add(unit);
        OnUnitPlaced?.Invoke(unit);
    }

    public void RemoveUnitFromList(Unit unit)
    {
        if (_demo)
            return;
        if (unit.Faction == UnitFaction.Player)
            PlayerUnits.Remove(unit);
        else EnemyUnits.Remove(unit);
        OnUnitRemoved?.Invoke(unit);
    }

    public void ClearBoard()
    {
        for (int i = PlayerUnits.Count - 1; i >= 0; i--)
        {
            var unit = PlayerUnits[i];
            if (unit == null)
                continue;
            Destroy(unit.gameObject);
            PlayerUnits.Remove(unit);
        }
        for (int i = EnemyUnits.Count - 1; i >= 0; i--)
        {
            var unit = EnemyUnits[i];
            if (unit == null)
                continue;
            Destroy(unit.gameObject);
            PlayerUnits.Remove(unit);
        }
    }
}

public interface IBoard<T> where T : IBoardTile
{
    public T[] TileArray { get; }

    public T GetTileAtPosition(Vector2 position);
}
