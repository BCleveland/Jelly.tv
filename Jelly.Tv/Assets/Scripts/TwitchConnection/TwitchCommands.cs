using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TwitchLib.Client.Events;


public class TwitchCommand
{
	public string CommandName { get; set; }
	public ECommandType m_commandType = ECommandType.Default;
	public delegate void Action(object sender, OnChatCommandReceivedArgs e);
	public delegate void WhisperAction(object sender, OnWhisperCommandReceivedArgs e);

	public Action command = null;
	public WhisperAction whisperCommand = null;

	public TwitchCommand(string name, Action command, ECommandType type = ECommandType.Default)
	{
		CommandName = name;
		this.command = command;
	}

	public TwitchCommand(string name, WhisperAction command, ECommandType type = ECommandType.Default)
	{
		CommandName = name;
		this.whisperCommand = command;
	}
}


public enum ECommandType
{
	Default,
	Duel
}
