using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{
	Minigame m_CurrentMinigame;
	private void Awake()
	{
		m_CurrentMinigame = new ADSMinigame();

		//MinigameResult result = m_CurrentMinigame.ProcessGameLogic(null, 500, 50);
	}
	public bool IsValidCommand(string command)
	{
		if (m_CurrentMinigame != null)
			return m_CurrentMinigame.IsValidCommand(command);
		//if no minigame, then there aren't any minigame specific commands
		return false;
	}
}
