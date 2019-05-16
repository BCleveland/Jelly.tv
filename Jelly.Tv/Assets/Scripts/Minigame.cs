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
	public abstract MinigameResult ProcessGameLogic(List<Player> players, int potAmount, int participationMoney);
	
}

public class MinigameResult
{
	public string CoroutineToPlay;
	public Dictionary<int, PlayerOutcome> UserResults = new Dictionary<int, PlayerOutcome>();
	public Dictionary<int, int> UserEarnings = new Dictionary<int, int>();
}
public enum PlayerOutcome { Won, Lost, Tie }