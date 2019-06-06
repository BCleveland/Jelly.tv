using System.Collections;
using System.Collections.Generic;
using TwitchLib.Client.Events;
using UnityEngine;

public class CommandManager
{

	public List<TwitchCommand> Commands { get; set; }


	public CommandManager()
	{
		Commands = new List<TwitchCommand>();
		AddCommands();
	}

	private void AddCommands()
	{
		Commands.Add(new TwitchCommand("screm", Command_Screm));
		//login command
		Commands.Add(new TwitchCommand("login", Command_Login));
		//register command
		//info command
		Commands.Add(new TwitchCommand("info", Command_Info));
		Commands.Add(new TwitchCommand("winfo", WhisperCommand_Info));
		Commands.Add(new TwitchCommand("commands", Command_ListCommands));
	}

	private void Command_Login(object sender, OnChatCommandReceivedArgs e)
	{
		string message = UserInputManager.GetParsedMessage(UserInputManager.CommandFeedback_LoginReturning, e);

		TwitchClient.Instance.Client.SendMessage(TwitchClient.Instance.Client.JoinedChannels[0], message);
		PlayerManager.Instance.Login(e.Command.ChatMessage.UserId, e.Command.ChatMessage.Username);
	}
	private void Command_Screm(object sender, OnChatCommandReceivedArgs e)
	{
		TwitchClient.Instance.Client.SendMessage(TwitchClient.Instance.Client.JoinedChannels[0], "i screm");
	}

	private void Command_Info(object sender, OnChatCommandReceivedArgs e)
	{
		string message = UserInputManager.GetParsedMessage(UserInputManager.CommandFeedback_Info, e);
		//"Welcome to Merlin's channel! I'm currently building a very good bot boy with some friends!"
		TwitchClient.Instance.Client.SendMessage(TwitchClient.Instance.Client.JoinedChannels[0], message);
	}
	private void WhisperCommand_Info(object sender, OnWhisperCommandReceivedArgs e)
	{
		string message = UserInputManager.GetParsedMessage(UserInputManager.CommandFeedback_Info);
		//"Welcome to Merlin's channel! I'm currently building a very good bot boy with some friends!"
		TwitchClient.Instance.Client.SendWhisper(e.Command.WhisperMessage.Username, message);
		
		Debug.Log("whisper command info");
	}
	private void Command_ListCommands(object sender, OnChatCommandReceivedArgs e)
	{
		string commands = "Default Commands: \n";

		foreach (var item in GetAllCommandsofType(ECommandType.Default))
		{
			commands += item.CommandName + " ";
		}
		TwitchClient.Instance.Client.SendMessage(TwitchClient.Instance.Client.JoinedChannels[0], commands);
	}

	public List<TwitchCommand> GetAllCommandsofType(ECommandType type)
	{
		List<TwitchCommand> result = new List<TwitchCommand>();

		foreach (var item in Commands)
		{
			if (item.m_commandType == type) result.Add(item);
		}
		return result;
	}
}
