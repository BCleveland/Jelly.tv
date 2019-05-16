using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{
	private void Awake()
	{
		Minigame minigame = new ADSMinigame();
		List<Player> players = new List<Player>()
		{
			new Player(){TwitchID = 175123, LastCommand = "Steal"},
			new Player(){TwitchID = 591824, LastCommand = "Steal"}
		};

		MinigameResult result = minigame.ProcessGameLogic(players, 500, 50);
		Debug.Log("here!");
	}
}
