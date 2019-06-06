using Doozy.Engine.Nody;
using Priority_Queue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby : Singleton<Lobby> {

    [SerializeField] private int m_minSlimeCount = 0;
    [SerializeField] private Vector3 m_startingPosition;
    [SerializeField] private Vector3 m_slimeWanderRange;
    [SerializeField] private List<Vector3> m_fleeLocations = new List<Vector3>();
    [SerializeField] private Slime m_slimePrefab = null;

    private SimplePriorityQueue<PlayerManager.Player> m_playerQueue = new SimplePriorityQueue<PlayerManager.Player>();

    private List<Slime> m_slimes;


    public Vector3 StartingPosition { get => m_startingPosition; }
    public Vector3 WanderRange { get => m_slimeWanderRange; }
    public List<Vector3> FleeLocations { get => m_fleeLocations; }

    public SimplePriorityQueue<PlayerManager.Player> PlayerQueue { get => m_playerQueue; set => m_playerQueue = value; }

    void Start() {
        m_slimes = new List<Slime>();
        if (m_slimePrefab) {
            for (int i = 0; i < m_minSlimeCount; i++) {
                m_slimes.Add(Instantiate(m_slimePrefab, m_startingPosition, Quaternion.identity));
            }
        }
    }

    public PlayerManager.Player GetNextPlayer() {
        PlayerManager.Player player = PlayerQueue.Dequeue();
        PlayerQueue.Enqueue(player, 0.0f);
        return player;
    }

    public Slime GetSoullessSlime() {
        Slime slime = m_slimes.Find(s => s.PlayerID == "");
        if (!slime && m_slimePrefab) {
            slime = Instantiate(m_slimePrefab, m_startingPosition, Quaternion.identity);
            m_slimes.Add(slime);
        }
        return slime;
    }

    public void Logout(PlayerManager.Player player) {
        PlayerQueue.Remove(player);
        if (player.Slime) {
            m_slimes.Remove(player.Slime);
            Destroy(player.Slime);
        }
    }
}
