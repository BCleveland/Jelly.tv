using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager> {

    public class Player {
        #region Private Variables
        private int m_lastGameId;
        private int m_money;
        private string m_id;
        private string m_userName;
        private string m_miniGameCommand;
        #endregion
        public int LastGameId { get => m_lastGameId; set => m_lastGameId = value; }
        public int Money { get => m_money; set => m_money = value; }
        public string Id { get => m_id; set => m_id = value; }
        public string UserName { get => m_userName; set => m_userName = value; }
        public string MiniGameCommand { get => m_miniGameCommand; set => m_miniGameCommand = value; }
        public Slime Slime { get; set; }
        public string SlimeFace { get; set; }
        public string SlimeShape { get; set; }
        public string SlimeColor { get; set; }

        public Player(string id, int money = 0, string face = "basic", string shape = "basic", string slimeColor = "#FFFFFF") {
            Id = id;
            Money = money;
            SlimeFace = face;
            SlimeShape = shape;
            SlimeColor = slimeColor;
            FindObjectOfType<SlimeCustomization>().SetSlime(face, shape, Slime, slimeColor);
        }
    }

    [System.Serializable]
    public class PlayerData {
        public string Id { get; set; }
        public int Money { get; set; }
        public string SlimeFace { get; set; }
        public string SlimeShape { get; set; }
        public string SlimeColor { get; set; }
    }

    private Dictionary<string, Player> m_playerDictionary = new Dictionary<string, Player>();
    private List<Player> m_activePlayers = new List<Player>();

    public Dictionary<string, Player> PlayerDictioary { get => m_playerDictionary; }

    private void Start() {
        LoadPlayers();
    }

    public void SavePlayers() {
        List<PlayerData> players = new List<PlayerData>();
        foreach (var p in m_playerDictionary) {
            players.Add(new PlayerData() { Id = p.Value.Id, Money = p.Value.Money, SlimeFace = p.Value.SlimeFace, SlimeShape = p.Value.SlimeShape, SlimeColor = p.Value.SlimeColor });
        }

        string json = JsonUtility.ToJson(players);
        PlayerPrefs.SetString("PlayerData", json);
    }

    /// <summary>
    /// Adds a player to the active player list if they're not already in it.
    /// </summary>
    /// <param name="id">The Twitch Id of the player</param>
    public void Login(string id, string userName) {
        if (CheckPlayerExists(id)) {
            SetPlayerActive(id);
        } else {
            RegisterPlayer(id);
            SetPlayerActive(id);
        }
        m_playerDictionary[id].UserName = userName;
        Slime slime = Lobby.Instance.GetSoullessSlime();
        if (slime) {
            slime.SetPlayer(m_playerDictionary[id]);
            m_playerDictionary[id].Slime = slime;
        }
        Lobby.Instance.PlayerQueue.Enqueue(m_playerDictionary[id], 0.0f);
    }

    public void Logout(string id) {
        if (CheckPlayerExists(id)) {
            m_activePlayers.Remove(m_playerDictionary[id]);
            Lobby.Instance.Logout(m_playerDictionary[id]);
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
    private bool RegisterPlayer(string id, int money = 0, string face = "basic", string shape = "basic", string color = "#FFFFFF") => m_playerDictionary.AddUnique(id, new Player(id, money, face, shape, color));

    private void LoadPlayers() {
        List<PlayerData> players = JsonUtility.FromJson<List<PlayerData>>(PlayerPrefs.GetString("PlayerData"));
        if (players == null) SavePlayers();
        players = JsonUtility.FromJson<List<PlayerData>>(PlayerPrefs.GetString("PlayerData"));
        players.ForEach(p => {
            RegisterPlayer(p.Id, p.Money, p.SlimeFace, p.SlimeShape, p.SlimeColor);
        });
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