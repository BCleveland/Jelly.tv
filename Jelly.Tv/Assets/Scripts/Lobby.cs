﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby : Singleton<Lobby> {

    [SerializeField] private int m_minSlimeCount = 0;
    [SerializeField] private Vector3 m_startingPosition;
    [SerializeField] private Slime m_slimePrefab = null;
    [SerializeField] private Claw m_leftClaw = null;
    [SerializeField] private Claw m_rightClaw = null;

    private Queue<PlayerManager.Player> m_playerQueue = new Queue<PlayerManager.Player>();
    private List<Slime> m_slimes;


    public Vector3 StartingPosition { get => m_startingPosition; }
    public Queue<PlayerManager.Player> PlayerQueue { get => m_playerQueue; set => m_playerQueue = value; }

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
        PlayerQueue.Enqueue(player);
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
}
