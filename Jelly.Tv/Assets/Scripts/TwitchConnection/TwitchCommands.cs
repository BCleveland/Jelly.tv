using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TwitchLib.Client.Events;


public class TwitchCommand
{
	public string CommandName { get; set; }
	public ECommandType m_commandType = ECommandType.Default;
	public delegate void Action(object sender, OnChatCommandReceivedArgs e);

	public Action command = null;

	public TwitchCommand(string name, Action command)
	{
		CommandName = name;
		this.command = command;
	}

}


public enum ECommandType
{
	Default,
	Duel
}
