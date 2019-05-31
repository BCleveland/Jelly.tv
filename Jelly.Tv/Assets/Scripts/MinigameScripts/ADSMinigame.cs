using System.Collections;
using System.Collections.Generic;
using TwitchLib.Client.Events;
using UnityEngine;

public class ADSMinigame : Minigame
{
	//static information
	private static float k_StealModifier = 1.5f;

	List<PlayerManager.Player> m_Players;
	int potAmount, participationMoney;
	public ADSMinigame(List<PlayerManager.Player> players, int PotAmount, int Participation)
	{
		MinigameCommands = new List<TwitchCommand>()
		{
			new TwitchCommand("attack", AttackCommand),
			new TwitchCommand("defend", DefendCommand),
			new TwitchCommand("steal", StealCommand)
		};
		m_Players = players;
		potAmount = PotAmount;
		participationMoney = Participation;
	}
	public override MinigameResult ProcessGameLogic()
	{
		//3x3 matrix of results. x is p1, y is p2 command
		string result = new string[][]
		{
			new string[]{"tie", "p1", "p2"},
			new string[]{"p2", "tie", "p1"},
			new string[]{"p1", "p2", "lose"}
		}[(int)Commands.ADSCommandDict[m_Players[1].MiniGameCommand]]
		 [(int)Commands.ADSCommandDict[m_Players[0].MiniGameCommand]];

		MinigameResult output = new MinigameResult() { CoroutineToPlay = "ADSResult" };

		if (result == "tie")
		{
			//split the pot
			int amount = Mathf.FloorToInt(potAmount / 2.0f) + participationMoney;
			foreach (PlayerManager.Player p in m_Players)
			{
				output.UserResults.Add(p.Id, PlayerOutcome.Tie);
				output.UserEarnings.Add(p.Id, amount);
			}
		}
		else if (result == "lose")
		{
			//neither player earns anything other than participation
			foreach(PlayerManager.Player p in m_Players)
			{
				output.UserResults.Add(p.Id, PlayerOutcome.Lost);
				output.UserEarnings.Add(p.Id, participationMoney);
			}
		}
		else
		{
			PlayerManager.Player winner = m_Players[(result == "p1") ? 0 : 1];
			PlayerManager.Player loser = m_Players[(result == "p1") ? 1 : 0];

			int winnerAmount = potAmount;
			if (winner.MiniGameCommand == "Steal")
				winnerAmount = Mathf.FloorToInt(winnerAmount * k_StealModifier);
			winnerAmount += participationMoney;

			output.UserResults.Add(winner.Id, PlayerOutcome.Won);
			output.UserEarnings.Add(winner.Id, winnerAmount);
			output.UserResults.Add(loser.Id, PlayerOutcome.Lost);
			output.UserEarnings.Add(loser.Id, participationMoney);
		}
		return output;
	}

	public override void RegisterCommands(List<TwitchCommand> commandList)
	{
		foreach(TwitchCommand command in MinigameCommands)
		{
			commandList.Add(command);
		}
	}

	public override void UnregisterCommands(List<TwitchCommand> commandList)
	{
		foreach (TwitchCommand command in MinigameCommands)
		{
			commandList.Remove(command);
		}
	}

	private void AttackCommand(object sender, OnChatCommandReceivedArgs e)
	{
		DoCommand(e.Command.ChatMessage.UserId, "Attack");
	}
	private void DefendCommand(object sender, OnChatCommandReceivedArgs e)
	{
		DoCommand(e.Command.ChatMessage.UserId, "Defend");
	}
	private void StealCommand(object sender, OnChatCommandReceivedArgs e)
	{
		DoCommand(e.Command.ChatMessage.UserId, "Steal");
	}
	private void DoCommand(string id, string command)
	{
		PlayerManager.Player player = m_Players.Find(p => p.Id == id);
		if (player.MiniGameCommand == "" || player.MiniGameCommand == null)
		{
			player.MiniGameCommand = command;
		}
		//check if all
		if(m_Players.FindAll(p => p.MiniGameCommand == "" || p.MiniGameCommand == null).Count == 0)
		{
			MinigameManager.Instance.MinigameOver(ProcessGameLogic());
		}
	}
}