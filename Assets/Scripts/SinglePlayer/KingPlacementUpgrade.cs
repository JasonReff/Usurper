using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Placement")]
public class KingPlacementUpgrade : SinglePlayerUpgrade
{
    public List<Vector2> AdditionalPlacementSpaces;
    [SerializeField] private KingUnitData _kingUnitData;

    public override void StartUpgrade()
    {
        _kingUnitData.AddAdditionalPlacement(AdditionalPlacementSpaces);
    }

    public override void EndUpgrade()
    {
        _kingUnitData.ClearAdditionalPlacementSpaces();
    }
}