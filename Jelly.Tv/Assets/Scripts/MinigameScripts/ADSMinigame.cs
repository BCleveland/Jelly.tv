using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADSMinigame : Minigame
{
	//static information
	private static float k_StealModifier = 1.5f;

	public override bool IsValidCommand(string command)
	{
		return Commands.ADSCommandDict.ContainsKey(command);
	}

	public override MinigameResult ProcessGameLogic(List<PlayerManager.Player> players, int potAmount, int participationMoney)
	{
		//3x3 matrix of results. x is p1, y is p2 command
		string result = new string[][]
		{
			new string[]{"tie", "p1", "p2"},
			new string[]{"p2", "tie", "p1"},
			new string[]{"p1", "p2", "lose"}
		}[(int)Commands.ADSCommandDict[players[1].MiniGameCommand]]
		 [(int)Commands.ADSCommandDict[players[0].MiniGameCommand]];

		MinigameResult output = new MinigameResult() { CoroutineToPlay = "ADSResult" };

		if (result == "tie")
		{
			//split the pot
			int amount = Mathf.FloorToInt(potAmount / 2.0f) + participationMoney;
			foreach (PlayerManager.Player p in players)
			{
				output.UserResults.Add(p.Id, PlayerOutcome.Tie);
				output.UserEarnings.Add(p.Id, amount);
			}
		}
		else if (result == "lose")
		{
			//neither player earns anything other than participation
			foreach(PlayerManager.Player p in players)
			{
				output.UserResults.Add(p.Id, PlayerOutcome.Lost);
				output.UserEarnings.Add(p.Id, participationMoney);
			}
		}
		else
		{
			PlayerManager.Player winner = players[(result == "p1") ? 0 : 1];
			PlayerManager.Player loser =  players[(result == "p1") ? 1 : 0];

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
}
