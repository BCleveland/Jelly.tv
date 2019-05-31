using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TwitchLib.Unity;
using TwitchLib.Client.Models;
using System;
using TwitchLib.Client.Events;

public class TwitchClient : Singleton<TwitchClient>
{
	public static Client client = null;
	[SerializeField] private string channel_name = "mightbeabitmagic";
	private CommandManager commandManager = null;
	private void Awake()
	{
		Application.runInBackground = true;

		//Init bot and tell to join
		ConnectionCredentials credidentials = new ConnectionCredentials("jellybottv", Keys.BotAccessToken);
		client = new Client();
		client.Initialize(credidentials, channel_name, '!', '!', false);

		Commands = new List<TwitchCommand>();
		AddCommands();

		client.Connect();
		client.OnConnected += ConnectedtoChannel;
		client.OnMessageReceived += MyMessageReceivedFunction;
		client.OnChatCommandReceived += CommandRecieved;

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
		client.SendMessage(client.JoinedChannels[0], "i screm");
	}

	private void InfoCommand(object sender, OnChatCommandReceivedArgs e)
	{
		client.SendMessage(client.JoinedChannels[0], "Welcome to Merlin's channel! I'm currently building a very good bot boy with some friends!");
	}

	private void ConnectedtoChannel(object sender, OnConnectedArgs e)
	{
		client.SendMessage(client.JoinedChannels[0], "owo jelly boi is here!");
	}

	private void CommandRecieved(object sender, OnChatCommandReceivedArgs e)
	{
		Debug.Log(e.Command.ChatMessage.Username + ": " + e.Command.ChatMessage.UserId);
		client.SendMessage(client.JoinedChannels[0], "Beep boop. Command recieved! " + e.Command.CommandText);
		for(int i = 0; i < Commands.Count; i++)
		{
			if (Commands[i].CommandName == e.Command.CommandText)
			{
				Commands[i].command.Invoke(sender, e);
			}
		}
		Debug.Log(e.Command.ChatMessage.Username + ": " + e.Command.ChatMessage.UserId);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			client.SendMessage(client.JoinedChannels[0], "YEETHAW");
		}
	}

	private void MyMessageReceivedFunction(object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
	{
		Debug.Log("The bot just read a message in chat");
	}


}
