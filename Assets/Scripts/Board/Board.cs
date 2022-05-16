using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static Board Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] private BoardTile[] tileArray;

    public static BoardTile? GetTileAtPosition(Vector2 position)
    {
        if (Instance.tileArray.Where(t => (Vector2)t.transform.localPosition == position).Count() == 1)
            return Instance.tileArray.First(t => (Vector2)t.transform.localPosition == position);
        else return null;
    }
}