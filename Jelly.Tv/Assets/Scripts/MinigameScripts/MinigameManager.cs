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
	public bool IsMinigameStarting = false;
	private int m_LastMinigameId = -1;
	private Minigame m_CurrentMinigame;
	private void Update()
	{
		if(IsMinigameStarting)//Input.GetKeyDown(KeyCode.M))
		{
			IsMinigameStarting = false;
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
		participants.ForEach(p => p.Slime.TextDisplay.text = "Waiting for Command...");
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
		SetAllSlimesToState("Flee");
		//Claws to grab slimes
		Vector3[] targetPositions = new Vector3[]
		{
			new Vector3(-3, -4.5f, 0),
			new Vector3(3, -4.5f, 0)
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
		Vector3[] targetPositions = new Vector3[]
{
			new Vector3(-5, -4.5f, 0),
			new Vector3(5, -4.5f, 0)
};
		//Claws to grab slimes
		List<Coroutine> activeCoroutines = new List<Coroutine>();
		for (int i = 0; i < result.Participants.Count; i++)
		{
			activeCoroutines.Add(StartCoroutine(m_Claws[i].MoveToAndGrabSlime(result.Participants[i].Slime)));
		}
		//wait for finish
		foreach (var c in activeCoroutines)
		{
			yield return c;
		}
		activeCoroutines.Clear();
		//claws move slimes to random position while background transitions
		for (int i = 0; i < result.Participants.Count; i++)
		{
			activeCoroutines.Add(StartCoroutine(
				m_Claws[i].MoveAboveDesiredPos(result.Participants[i].Slime, targetPositions[i])));
		}
		m_GraphController.Graph.SetActiveNodeByName("GameUI");
		//wait for finish
		foreach (var c in activeCoroutines)
		{
			yield return c;
		}
		activeCoroutines.Clear();
		//drop slimes and return
		for (int i = 0; i < result.Participants.Count; i++)
		{
			activeCoroutines.Add(StartCoroutine(
				m_Claws[i].DropAndMoveBack(result.Participants[i].Slime)));
		}
		//wait for finish
		foreach (var c in activeCoroutines)
		{
			yield return c;
		}
		SetAllSlimesToState("Wander");
		result.Participants.ForEach(p => p.Slime.TextDisplay.text = p.UserName);
	}
	private IEnumerator ADSMinigameResult(MinigameResult result)
	{
		//Slimes both do their animation
		foreach (PlayerManager.Player player in result.Participants)
		{
			player.Slime.TextDisplay.text = player.MiniGameCommand;
			switch (player.MiniGameCommand)
			{
				case "attack":
					break;
				case "defend":
					break;
				case "steal":
					break;
			}
		}
		yield return new WaitForSeconds(1.0f);
		//React accordingly to win/loss
		foreach (PlayerManager.Player player in result.Participants)
		{
			player.Slime.TextDisplay.text = result.UserResults[player.Id].ToString();
			switch(result.UserResults[player.Id])
			{
				case PlayerOutcome.Won:
					break;
				case PlayerOutcome.Lost:
					break;
				case PlayerOutcome.Tie:
					break;
			}
		}
		yield return new WaitForSeconds(1.0f);
		result.Participants.ForEach(p => p.MiniGameCommand = "");
		//display earned money count
		foreach(PlayerManager.Player player in result.Participants)
		{
			player.Slime.TextDisplay.text = "$" + result.UserEarnings[player.Id];
		}
		yield return new WaitForSeconds(2.0f);
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
	private void SetAllSlimesToState(string state)
	{
		foreach(var player in PlayerManager.Instance.PlayerDictioary.Values)
		{
			player.Slime.State = state;
		}
	}
}
