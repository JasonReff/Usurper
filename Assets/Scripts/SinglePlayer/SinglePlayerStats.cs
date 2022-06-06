using UnityEngine;

[CreateAssetMenu(menuName = "SinglePlayerStats")]
public class SinglePlayerStats : ScriptableObject
{
    public int Round = 1;

    public void ResetStats()
    {
        Round = 1;
    }
}
