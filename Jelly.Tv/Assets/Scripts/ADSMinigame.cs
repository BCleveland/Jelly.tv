using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADSMinigame : Minigame
{
	//static information
	private static float k_StealModifier = 1.5f;

	public override MinigameResult ProcessGameLogic(List<Player> players, int potAmount, int participationMoney)
	{
		//3x3 matrix of results. x is p1, y is p2 command
		string result = new string[][]
		{
			new string[]{"tie", "p1", "p2"},
			new string[]{"p2", "tie", "p1"},
			new string[]{"p1", "p2", "lose"}
		}[(int)Commands.ADSCommandDict[players[1].LastCommand]]
		 [(int)Commands.ADSCommandDict[players[0].LastCommand]];

		MinigameResult output = new MinigameResult() { CoroutineToPlay = "ADSResult" };

		if (result == "tie")
		{
			//split the pot
			int amount = Mathf.FloorToInt(potAmount / 2.0f) + participationMoney;
			foreach (Player p in players)
			{
				output.UserResults.Add(p.TwitchID, PlayerOutcome.Tie);
				output.UserEarnings.Add(p.TwitchID, amount);
			}
		}
		else if (result == "lose")
		{
			//neither player earns anything other than participation
			foreach(Player p in players)
			{
				output.UserResults.Add(p.TwitchID, PlayerOutcome.Lost);
				output.UserEarnings.Add(p.TwitchID, participationMoney);
			}
		}
		else
		{
			Player winner = players[(result == "p1") ? 0 : 1];
			Player loser =  players[(result == "p1") ? 1 : 0];

			int winnerAmount = potAmount;
			if (winner.LastCommand == "Steal")
				winnerAmount = Mathf.FloorToInt(winnerAmount * k_StealModifier);
			winnerAmount += participationMoney;

			output.UserResults.Add(winner.TwitchID, PlayerOutcome.Won);
			output.UserEarnings.Add(winner.TwitchID, winnerAmount);
			output.UserResults.Add(loser.TwitchID, PlayerOutcome.Lost);
			output.UserEarnings.Add(loser.TwitchID, participationMoney);
		}
		return output;
	}
}
