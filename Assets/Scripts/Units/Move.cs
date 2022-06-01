public class Move
{
    protected IUnit _unit;
    private UnitData _unitData;
    private IBoardTile _currentTile, _newTile;
    public int Evaluation;

    public IBoardTile NewTile { get => _newTile; }
    public IBoardTile CurrentTile { get => _currentTile; }

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
            _currentTile.Unit = null;
        var otherUnit = _newTile.Unit;
        if (otherUnit != null)
            unit.CaptureUnit(otherUnit as Unit);
        _newTile.Unit = unit;
        unit.Tile = _newTile as BoardTile;
    }

    public void RangedAttack(Unit unit)
    {
        var otherUnit = _newTile.Unit;
        if (otherUnit != null)
        {
            _newTile.Unit = null;
            unit.CaptureUnit(otherUnit as Unit);
        }
    }

}
