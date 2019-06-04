using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TwitchLib.Unity;
using TwitchLib.Client.Models;
using System;
using TwitchLib.Client.Events;

public class TwitchClient : Singleton<TwitchClient>
{

    [SerializeField] private string channel_name = "mightbeabitmagic";

    private Client m_client = null;
	private CommandManager m_commandManager = null;

	public Client Client { get => m_client; private set => m_client = value; }
	public CommandManager CommandManager { get => m_commandManager; private set => m_commandManager = value; }


	private void Awake()
	{
		Application.runInBackground = true;

		m_commandManager = new CommandManager();

		//Init bot and tell to join
		ConnectionCredentials credidentials = new ConnectionCredentials("jellybottv", Keys.BotAccessToken);
		Client = new Client();
		Client.Initialize(credidentials, channel_name, '!', '!', false);

		Client.Connect();
		Client.OnConnected += ConnectedtoChannel;
		//client.OnMessageReceived += MyMessageReceivedFunction;
		Client.OnChatCommandReceived += CommandRecieved;

	}


	private void ConnectedtoChannel(object sender, OnConnectedArgs e)
	{
		Client.SendMessage(Client.JoinedChannels[0], "owo jelly boi is here!");
	}

	private void CommandRecieved(object sender, OnChatCommandReceivedArgs e)
	{
		Debug.Log(e.Command.ChatMessage.Username + ": " + e.Command.ChatMessage.UserId);
		Client.SendMessage(Client.JoinedChannels[0], "Beep boop. Command recieved! " + e.Command.CommandText);
		for(int i = 0; i < m_commandManager.Commands.Count; i++)
		{
			if (m_commandManager.Commands[i].CommandName == e.Command.CommandText)
			{
				m_commandManager.Commands[i].command.Invoke(sender, e);
			}
		}
		Debug.Log(e.Command.ChatMessage.Username + ": " + e.Command.ChatMessage.UserId);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			Client.SendMessage(Client.JoinedChannels[0], "YEETHAW");
		}
	}

}
