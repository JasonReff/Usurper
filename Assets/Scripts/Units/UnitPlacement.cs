public class UnitPlacement : Move
{
    public IUnit Unit => _unit;
    public UnitPlacement(IUnit unit, IBoardTile placedTile) : base(unit, placedTile)
    {
        
    }
}
