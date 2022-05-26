using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardPool")]
public class CardPool : ScriptableObject
{
    public List<UnitData> UnitPool;
    public List<UnitData> PawnPool;
}