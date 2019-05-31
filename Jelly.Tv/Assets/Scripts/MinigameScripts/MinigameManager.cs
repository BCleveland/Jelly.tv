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
	private void Update()
	{
		if (m_CurrentMinigame == null && Lobby.Instance.PlayerQueue.Count >= 2)
		{
			List<PlayerManager.Player> players = new List<PlayerManager.Player>()
			{
				Lobby.Instance.GetNextPlayer(),
				Lobby.Instance.GetNextPlayer()
			};
			StartMinigame(players);
		}
	}
	public void StartMinigame(List<PlayerManager.Player> participants)
	{
		Debug.Log("Minigame Starting!");
		m_LastMinigameId++;
		//todo
		//get the int of currentMinigame - player last minigame
		float longestInQueue = 0.0f;
		float queueMultiplier = 1.0f + (longestInQueue * m_QueueMultiplier);
		int participationAmount = Mathf.FloorToInt(m_ParticipationMoney * queueMultiplier);
		int victoryAmount = Mathf.FloorToInt(m_WinMoney * queueMultiplier);
		m_CurrentMinigame = new ADSMinigame(participants, victoryAmount, participationAmount);
		TwitchClient.Instance.client.SendMessage(TwitchClient.Instance.client.JoinedChannels[0], "Starting Minigame!" + 
			victoryAmount);
		RegisterCommands();
	}
	public void MinigameOver(MinigameResult result)
	{
		//do animations here
		Debug.Log("MINIGAME HAS BEEN PLAYED");
		string output = "";
		foreach (string id in result.UserResults.Keys)
		{
			Debug.Log(id + ": " + result.UserResults[id]);
			output += id + ": " + result.UserResults[id];
		}
		TwitchClient.Instance.client.SendMessage(TwitchClient.Instance.client.JoinedChannels[0], output);

UnregisterCommands();
		m_CurrentMinigame = null;
	}
	private void RegisterCommands()
	{
		var commandList = TwitchClient.Instance.Commands;
		m_CurrentMinigame.RegisterCommands(commandList);
	}
	private void UnregisterCommands()
	{
		var commandList = TwitchClient.Instance.Commands;
		m_CurrentMinigame.UnregisterCommands(commandList);
	}
}
