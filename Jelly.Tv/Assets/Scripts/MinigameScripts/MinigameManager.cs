using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameManager : Singleton<MinigameManager>
{
	[Header("Economy Settings")]
	[SerializeField] private int m_ParticipationMoney = 50;
	[SerializeField] private int m_WinMoney = 500;
	[Tooltip("The multiplier per game the player was in queue")]
	[SerializeField] private float m_QueueMultiplier = 0.1f;

	private int m_LastMinigameId = -1;
	private Minigame m_CurrentMinigame;
	private void Awake()
	{
		SetupNextMinigame();
	}
	public void SetupNextMinigame()
	{
		m_LastMinigameId++;
		m_CurrentMinigame = new ADSMinigame();
	}
	public void StartMinigame(List<Player> participants)
	{
		//todo
		//get the int of currentMinigame - player last minigame
		float longestInQueue = 0.0f;
		float queueMultiplier = 1.0f + (longestInQueue * m_QueueMultiplier);
		int participationAmount = Mathf.FloorToInt(m_ParticipationMoney * queueMultiplier);
		int victoryAmount = Mathf.FloorToInt(m_WinMoney * queueMultiplier);
		MinigameResult result = m_CurrentMinigame.
			ProcessGameLogic(participants, victoryAmount, participationAmount);
		Debug.Log("MINIGAME HAS BEEN PLAYED");
		foreach(string id in result.UserResults.Keys)
		{
			Debug.Log(id + ": " + result.UserResults[id]);
		}
	}
	public bool IsValidCommand(string command)
	{
		if (m_CurrentMinigame != null)
			return m_CurrentMinigame.IsValidCommand(command);
		//if no minigame, then there aren't any minigame specific commands
		return false;
	}

}
