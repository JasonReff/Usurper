using UnityEngine;

public abstract class SinglePlayerUpgrade : ScriptableObject
{
    [SerializeField] private Sprite _upgradeSprite;
    public Sprite UpgradeSprite { get => _upgradeSprite; }
    [SerializeField] private string _upgradeName;
    public string UpgradeName { get => _upgradeName; }

    public virtual void StartUpgrade()
    {

    }

    public virtual void EndUpgrade()
    {

    }
}
