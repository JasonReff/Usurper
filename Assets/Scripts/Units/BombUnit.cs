using System.Collections;

public class BombUnit : Unit
{
    protected override IEnumerator UnitDeath()
    {
        if (_hasExploded == true)
            yield break;
        yield return base.UnitDeath();
        Explode();
    }
}