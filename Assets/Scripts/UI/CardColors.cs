using UnityEngine;

[CreateAssetMenu(menuName = "CardColors")]
public class CardColors : ScriptableObject
{
    public CardColorSet Kingdom, Shogunate, Empire, Neutral;
}

[System.Serializable]
public class CardColorSet
{
    public Sprite WhiteCard, BlackCard, Cost, ButtonUp, ButtonDown;
}