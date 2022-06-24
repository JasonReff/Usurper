using UnityEngine;

public abstract class SinglePlayerSpecialEncounter : MonoBehaviour
{
    [SerializeField] private string _encounterName;

    public string EncounterName { get => _encounterName; }
    public virtual void StartEncounter()
    {

    }

    public virtual void EndEncounter()
    {

    }

    private void OnDisable()
    {
        EndEncounter();
    }
}
