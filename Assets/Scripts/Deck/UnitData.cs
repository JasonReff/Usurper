using UnityEngine;

[CreateAssetMenu(menuName = "UnitData")]
public class UnitData : ScriptableObject
{
    public Unit Unit;
    public Sprite PlayerSprite, EnemySprite, Moveset;
    public int Cost;
    public string UnitName;

    public Sprite GetSprite(UnitFaction faction)
    {
        if (faction == UnitFaction.Player)
            return PlayerSprite;
        return EnemySprite;
    }
}