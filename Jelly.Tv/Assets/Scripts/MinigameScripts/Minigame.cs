using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Minigame process:
 * Game begins and selects random players to participate
 * Once both players have submitted a command, run game logic
 * After processing, do visual animations to showcase the result
 * reset back to original state
 */

public abstract class Minigame
{
	public abstract MinigameResult ProcessGameLogic();
	public abstract void RegisterCommands(List<TwitchCommand> commandList);
	public abstract void UnregisterCommands(List<TwitchCommand> commandList);
	public List<TwitchCommand> MinigameCommands;
	protected void DishOutEarnings(List<PlayerManager.Player> players, MinigameResult result)
	{
		foreach(var player in players)
		{
			player.Money += result.UserEarnings[player.Id];
			player.MiniGameCommand = "";
		}
	}
}

public class MinigameResult
{
	public string CoroutineToPlay;
	public Dictionary<string, PlayerOutcome> UserResults = new Dictionary<string, PlayerOutcome>();
	public Dictionary<string, int> UserEarnings = new Dictionary<string, int>();
}
public enum PlayerOutcome { Won, Lost, Tie }