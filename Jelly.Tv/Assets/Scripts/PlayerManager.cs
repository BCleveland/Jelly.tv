using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager> {

    public class Player {
        private int m_lastGameId;
        public int LastGameId { get => m_lastGameId; set => m_lastGameId = value; }
        public int Money { get; set; }
        public string Id { get; set; }
        public string MiniGameCommand { get; set; }

        public Player(string id, int money = 0) {
            Id = id;
            Money = money;
        }
    }

    [System.Serializable]
    public class PlayerData {
        public string Id { get; set; }
        public int Money { get; set; }
    }

    private Dictionary<string, Player> m_playerDictionary;
    private List<Player> m_activePlayers = new List<Player>();

    private void Start() {
        LoadPlayers();
    }

    public void SavePlayers() {
        List<PlayerData> players = new List<PlayerData>();
        foreach (var p in m_playerDictionary) {
            players.Add(new PlayerData() { Id = p.Value.Id, Money = p.Value.Money });
        }

        string json = JsonUtility.ToJson(players);
        PlayerPrefs.SetString("PlayerData", json);
    }

    /// <summary>
    /// Adds a player to the active player list if they're not already in it.
    /// </summary>
    /// <param name="id">The Twitch Id of the player</param>
    public void Login(string id) {
        if (CheckPlayerExists(id)) {
            SetPlayerActive(id);
        } else {
            RegisterPlayer(id);
            SetPlayerActive(id);
        }
    }


    private void SetPlayerActive(string id) {
        if (!m_activePlayers.Contains(m_playerDictionary[id])) {
            m_activePlayers.Add(m_playerDictionary[id]);
        } else {
            Debug.Log($"User {id} is already playing the game.");
        }
    }

    /// <summary>
    /// Sets player inactive in game if they are active.
    /// </summary>
    /// <param name="id">The Twitch Id of the player</param>
    private void SetPlayerInactive(string id) => m_activePlayers.Remove(m_playerDictionary[id]);
    /// <summary>
    /// Checks if a player exists in the player dictionary.
    /// </summary>
    /// <param name="id">The Twitch Id of the player</param>
    /// <returns>Returns true if a player already exists and false if they don't.</returns>
    private bool CheckPlayerExists(string id) => m_playerDictionary.ContainsKey(id);
    /// <summary>
    /// Registers a new Player if they aren't already registered.
    /// </summary>
    /// <param name="id">The Twitch Id of the player</param>
    /// <returns>Retrns true if the player has been registered and false if the player is already registered.</returns>
    private bool RegisterPlayer(string id) => m_playerDictionary.AddUnique(id, new Player(id));

    private void LoadPlayers() {
        List<PlayerData> players = JsonUtility.FromJson<List<PlayerData>>(PlayerPrefs.GetString("PlayerData"));
    }


}

public static class HelperExtensions {
    public static bool AddUnique<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value) {
        if (!dictionary.ContainsKey(key)) {
            dictionary.Add(key, value);
            return true;
        } else {
            return false;
        }
    }
}