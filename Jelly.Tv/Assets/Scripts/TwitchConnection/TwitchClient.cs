using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TwitchLib.Unity;
using TwitchLib.Client.Models;
using System;
using TwitchLib.Client.Events;

public class TwitchClient : MonoBehaviour
{
	public Client client = null;
	[SerializeField] private string channel_name = "mightbeabitmagic";

	protected List<TwitchCommand> m_commands = null;

	private void Awake()
	{
		Application.runInBackground = true;

		//Init bot and tell to join
		ConnectionCredentials credidentials = new ConnectionCredentials("jellybottv", Keys.BotAccessToken);
		client = new Client();
		client.Initialize(credidentials, channel_name, '!', '!', false);

		m_commands = new List<TwitchCommand>();
		AddCommands();

		client.Connect();
		client.OnConnected += ConnectedtoChannel;
		client.OnMessageReceived += MyMessageReceivedFunction;
		client.OnChatCommandReceived += CommandRecieved;
	}

	private void AddCommands()
	{
		m_commands.Add(new TwitchCommand("screm", Screm));
		//login command
		//register command
		//info command
		m_commands.Add(new TwitchCommand("info", InfoCommand));

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
		
		client.SendMessage(client.JoinedChannels[0], "Beep boop. Command recieved! " + e.Command.CommandText);
		foreach (var item in m_commands)
		{
			if (item.CommandName == e.Command.CommandText)
			{
				item.command.Invoke(sender, e);
			}
		}
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
