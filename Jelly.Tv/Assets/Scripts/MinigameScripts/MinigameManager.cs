using Doozy.Engine.Nody;
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
	[SerializeField] Claw[] m_Claws = null;
	[SerializeField] GraphController m_GraphController = null;

	public bool IsInMinigame { get { return m_CurrentMinigame != null; } }

	private int m_LastMinigameId = -1;
	private Minigame m_CurrentMinigame;
	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.M))
		{
			List<PlayerManager.Player> players = new List<PlayerManager.Player>()
			{
				Lobby.Instance.GetNextPlayer(),
				Lobby.Instance.GetNextPlayer()
			};
			StartCoroutine(MinigameTransition(players));
		}
	}
	public void StartMinigame(List<PlayerManager.Player> participants)
	{
		Debug.Log("Minigame Starting!");
		m_LastMinigameId++;
		int longestInQueue = 0;
		participants.ForEach(p =>
		{
			if (m_LastMinigameId - p.LastGameId > longestInQueue)
			{
				longestInQueue = m_LastMinigameId - p.LastGameId;
			}
			p.LastGameId = m_LastMinigameId;
		});
		float queueMultiplier = 1.0f + (longestInQueue * m_QueueMultiplier);
		int participationAmount = Mathf.FloorToInt(m_ParticipationMoney * queueMultiplier);
		int victoryAmount = Mathf.FloorToInt(m_WinMoney * queueMultiplier);
		m_CurrentMinigame = new ADSMinigame(participants, victoryAmount, participationAmount);
		TwitchClient.Instance.Client.SendMessage(TwitchClient.Instance.Client.JoinedChannels[0], "Starting Minigame!" + 
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
		TwitchClient.Instance.Client.SendMessage(TwitchClient.Instance.Client.JoinedChannels[0], output);
		UnregisterCommands();
		StartCoroutine(result.CoroutineToPlay, result);
		m_CurrentMinigame = null;
	}
	private IEnumerator MinigameTransition(List<PlayerManager.Player> participants)
	{
		//Claws to grab slimes
		Vector3[] targetPositions = new Vector3[]
		{
			new Vector3(-5, 0, 0),
			new Vector3(5, 0, 0)
		};
		List<Coroutine> activeCoroutines = new List<Coroutine>();
		for(int i = 0; i < participants.Count; i++)
		{
			activeCoroutines.Add(StartCoroutine(m_Claws[i].MoveToAndGrabSlime(participants[i].Slime)));
		}
		//wait for finish
		foreach(var c in activeCoroutines)
		{
			yield return c;
		}

		//Claws move slime to desired position while background transitions
		activeCoroutines.Clear();
		for(int i = 0; i < participants.Count; i++)
		{
			activeCoroutines.Add(StartCoroutine(
				m_Claws[i].MoveAboveDesiredPos(participants[i].Slime, targetPositions[i])));
		}
		//TODO: add background transition to activeCoroutines
		m_GraphController.Graph.SetActiveNodeByName("BattleUI");
		//wait for finish
		foreach (var c in activeCoroutines)
		{
			yield return c;
		}

		//claws release slimes and leave screen via the side
		activeCoroutines.Clear();
		for (int i = 0; i < participants.Count; i++)
		{
			activeCoroutines.Add(StartCoroutine(
				m_Claws[i].DropAndMoveBack(participants[i].Slime)));
		}
		//wait for finish
		foreach (var c in activeCoroutines)
		{
			yield return c;
		}
		//TODO: Minigame visuals appear

		StartMinigame(participants);
	}
	private IEnumerator ReturnToLobbyTransition(MinigameResult result)
	{
		//Claws to grab slimes
		//claws move slimes to random position while background transitions
		m_GraphController.Graph.SetActiveNodeByName("GameUI");
		//claws release slimes and leave screen via the top
		//Other slimes enter somehow?
		yield return null;
	}
	private IEnumerator ADSMinigameResult(MinigameResult result)
	{
		//Slimes both do their animation
		//React accordingly to win/loss
		//display earned money count
		yield return null;
		StartCoroutine(ReturnToLobbyTransition(result));
	}

	private void RegisterCommands()
	{
		var commandList = TwitchClient.Instance.CommandManager.Commands;
		m_CurrentMinigame.RegisterCommands(commandList);
	}
	private void UnregisterCommands()
	{
		var commandList = TwitchClient.Instance.CommandManager.Commands;
		m_CurrentMinigame.UnregisterCommands(commandList);
	}
}
