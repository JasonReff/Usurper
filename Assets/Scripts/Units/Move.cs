using DG.Tweening;
using UnityEngine;

public class Move
{
    protected IUnit _unit;
    private UnitData _unitData;
    private IBoardTile _currentTile, _newTile;
    public int Evaluation;

    public IBoardTile NewTile { get => _newTile; }
    public IBoardTile CurrentTile { get => _currentTile; }
    public IUnit Unit { get => _unit; }

    public Move(UnitData unit, IBoardTile currentTile, IBoardTile nextTile)
    {
        _unitData = unit;
        _currentTile = currentTile;
        _newTile = nextTile;
    }

    public Move(IUnit unit, IBoardTile nextTile)
    {
        if (unit as Unit)
            _unit = unit as Unit;
        _newTile = nextTile;
    }

    public virtual void ExecuteMove(Unit unit)
    {
        if (_currentTile != null)
            _currentTile.UnitOnTile = null;
        var otherUnit = _newTile.UnitOnTile;
        if (otherUnit != null)
            unit.CaptureUnit(otherUnit as Unit);
        _newTile.UnitOnTile = unit;
        unit.Tile = _newTile as BoardTile;
    }

    public void RangedAttack(Unit unit)
    {
        var otherUnit = _newTile.UnitOnTile;
        if (otherUnit != null)
        {
            _newTile.UnitOnTile = null;
            unit.CaptureUnit(otherUnit as Unit);
            unit.transform.DORotate(new Vector3(0, 0, -45), 0.35f).OnComplete(() =>
            {
                unit.transform.DORotate(Vector3.zero, 0.35f);
            });
        }
    }

}
