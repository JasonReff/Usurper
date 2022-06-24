using System.Collections.Generic;
using UnityEngine;

public class SinglePlayerSpecialEncounterManager : MonoBehaviour
{
    [SerializeField] private List<SinglePlayerSpecialEncounter> _encounters = new List<SinglePlayerSpecialEncounter>();
    [SerializeField] private float _encounterProbability = 0.2f;

    private void Start()
    {
        var randomFloat = UnityEngine.Random.Range(0, 1f);
        if (randomFloat < _encounterProbability)
            BeginRandomEvent();
    }

    private void BeginRandomEvent()
    {
        var encounter = _encounters.Rand();
        encounter.StartEncounter();
    }
}
