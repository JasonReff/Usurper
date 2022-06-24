using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "CustomDecks/DeckCollection")]
public class CustomDeckCollection : ScriptableObject
{
    [SerializeField] private List<StartingDeck> _decks = new List<StartingDeck>();
    [SerializeField] private CardPool _pool;

    public List<StartingDeck> Decks { get => _decks; }

    private void OnEnable()
    {
        DeckPage.OnDeckSaved += SaveDecks;
        SavedDeckUI.OnDeckNameSaved += SaveDecks;
        SavedDeckUI.OnDeckCleared += SaveDecks;
    }

    private void OnDisable()
    {
        DeckPage.OnDeckSaved -= SaveDecks;
        SavedDeckUI.OnDeckNameSaved -= SaveDecks;
        SavedDeckUI.OnDeckCleared -= SaveDecks;
    }

    public void SaveDecks()
    {
        var deckList = new SerializableDeckList() { Decks = new List<SerializableDeck>() };
        for (int i = 0; i < _decks.Count; i++)
        {
            var deck = _decks[i];
            var serializedDeck = deck.Serialize();
            deckList.Decks.Add(serializedDeck);
        }
        var serializedDecksData = JsonUtility.ToJson(deckList);
        WriteToFile(serializedDecksData);
    }

    public void SaveDecks(SavedDeckUI deckUI)
    {
        var deckList = new SerializableDeckList() { Decks = new List<SerializableDeck>() };
        for (int i = 0; i < _decks.Count; i++)
        {
            var deck = _decks[i];
            var serializedDeck = deck.Serialize();
            deckList.Decks.Add(serializedDeck);
        }
        var serializedDecksData = JsonUtility.ToJson(deckList);
        WriteToFile(serializedDecksData);
    }

    public void LoadDecks()
    {
        string serializedDecksData = ReadDataFromFile();
        if (serializedDecksData == null)
            return;
        SerializableDeckList serializedDecks = JsonUtility.FromJson<SerializableDeckList>(serializedDecksData);
        for (int i = 0; i < serializedDecks.Decks.Count; i++)
        {
            var deck = serializedDecks.Decks[i];
            if (deck.Units.Count == 9)
                _decks[i].Deserialize(deck, _pool);
        }
    }

    private string ReadDataFromFile()
    {
        string filePath = Application.persistentDataPath + "/decks.txt";
        if (!File.Exists(filePath))
            return null;
        var reader = new StreamReader(filePath);
        string textData = reader.ReadToEnd();
        reader.Close();
        return textData;
    }

    private void WriteToFile(string deckData)
    {
        string filePath = Application.persistentDataPath + "/decks.txt";
        var writer = new StreamWriter(filePath);
        writer.WriteLine(deckData);
        writer.Close();
    }
}

[System.Serializable]
public struct SerializableDeckList
{
    public List<SerializableDeck> Decks;
}