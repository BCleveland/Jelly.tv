using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby : Singleton<Lobby> {

    [SerializeField] private int m_minSlimeCount = 10;
    [SerializeField] private Vector3 m_startingPosition;
    [SerializeField] private Slime m_slimePrefab = null;
    [SerializeField] private Claw m_leftClaw = null;
    [SerializeField] private Claw m_rightClaw = null;

    private Queue<PlayerManager.Player> m_playerQueue;
    private List<Slime> m_slimes;


    public Vector3 StartingPosition { get => m_startingPosition; }

    void Start() {
        m_slimes = new List<Slime>();
        if (m_slimePrefab) {
            for (int i = 0; i < m_minSlimeCount; i++) {
                m_slimes.Add(Instantiate(m_slimePrefab, m_startingPosition, Quaternion.identity));
            }
        }
    }

    // Update is called once per frame
    void Update() {

    }

    public Slime GetSoullessSlime() {
        Slime slime = m_slimes.Find(s => s.PlayerID == "");
        if (!slime) {
            slime = Instantiate(m_slimePrefab, m_startingPosition, Quaternion.identity);
            m_slimes.Add(slime);
        }
        return slime;
    }
}
