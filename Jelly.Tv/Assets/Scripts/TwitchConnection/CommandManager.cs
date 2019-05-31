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

	}

	private void AddCommands()
	{
		Commands.Add(new TwitchCommand("screm", Screm));
		//login command
		Commands.Add(new TwitchCommand("login", Login));
		//register command
		//info command
		Commands.Add(new TwitchCommand("info", InfoCommand));

	}

	private void Login(object sender, OnChatCommandReceivedArgs e)
	{
		PlayerManager.Instance.Login(e.Command.ChatMessage.UserId);
	}
	private void Screm(object sender, OnChatCommandReceivedArgs e)
	{
		TwitchClient.client.SendMessage(TwitchClient.client.JoinedChannels[0], "i screm");
	}

	private void InfoCommand(object sender, OnChatCommandReceivedArgs e)
	{
		TwitchClient.client.SendMessage(TwitchClient.client.JoinedChannels[0], "Welcome to Merlin's channel! I'm currently building a very good bot boy with some friends!");
	}
}
