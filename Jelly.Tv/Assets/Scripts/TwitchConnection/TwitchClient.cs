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

		client.Connect();
		client.OnConnected += ConnectedtoChannel;
		//client.OnMessageReceived += MyMessageReceivedFunction;
		client.OnChatCommandReceived += CommandRecieved;

	}


	private void ConnectedtoChannel(object sender, OnConnectedArgs e)
	{
		client.SendMessage(client.JoinedChannels[0], "owo jelly boi is here!");
	}

	private void CommandRecieved(object sender, OnChatCommandReceivedArgs e)
	{
		Debug.Log(e.Command.ChatMessage.Username + ": " + e.Command.ChatMessage.UserId);
		client.SendMessage(client.JoinedChannels[0], "Beep boop. Command recieved! " + e.Command.CommandText);
		for(int i = 0; i < commandManager.Commands.Count; i++)
		{
			if (commandManager.Commands[i].CommandName == e.Command.CommandText)
			{
				commandManager.Commands[i].command.Invoke(sender, e);
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

}
