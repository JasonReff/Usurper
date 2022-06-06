using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "CardPool")]
public class CardPool : ScriptableObject
{
    public List<UnitData> UnitPool;
    public List<UnitData> PawnPool;
    public List<UnitData> KingPool;

    public UnitData GetUnit(string unitName)
    {
        var allUnits = new List<UnitData>();
        allUnits.AddRange(UnitPool);
        allUnits.AddRange(PawnPool);
        allUnits.AddRange(KingPool);
        return allUnits.First(t => t.UnitName == unitName);
    }
}