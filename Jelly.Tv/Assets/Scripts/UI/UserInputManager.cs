using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using TwitchLib.Client.Events;

class UserInputManager : MonoBehaviour
{
	[SerializeField] TMP_InputField[] fields = null;

	public static string HostChannelName = "mightbeabitmagic";
	public static string CommandFeedback_Info = "Welcome to Merlin's channel! I'm currently building a very good bot boy with some friends!";
	public static string CommandFeedback_BotEnters = "owo jellyboi is here!";
	public static string CommandFeedback_BotLeaves = "byebye!!";
	public static string CommandFeedback_LoginNew = "hello new friend!!";
	public static string CommandFeedback_LoginReturning = "welcome back $USER$ !!!";

	private void Awake()
	{
		if (PlayerPrefs.HasKey("HostChannelName"))
		{
			//Pull existing values from Player Pref if they exist
			HostChannelName = PlayerPrefs.GetString("HostChannelName");
			CommandFeedback_Info = PlayerPrefs.GetString("CommandFeedback_Info");
			CommandFeedback_BotEnters = PlayerPrefs.GetString("CommandFeedback_BotEnters");
			CommandFeedback_BotLeaves = PlayerPrefs.GetString("CommandFeedback_BotLeaves");
			CommandFeedback_LoginNew = PlayerPrefs.GetString("CommandFeedback_LoginNew");
			CommandFeedback_LoginReturning = PlayerPrefs.GetString("CommandFeedback_LoginReturning");

		}
		//Update textboxes to current states
		fields[0].text = HostChannelName;
		fields[1].text = CommandFeedback_Info;
		fields[2].text = CommandFeedback_BotEnters;
		fields[3].text = CommandFeedback_BotLeaves;
		fields[4].text = CommandFeedback_LoginNew;
		fields[5].text = CommandFeedback_LoginReturning;
	}

	public void SaveVariables()
	{
		if (fields.Length == 6) //6 = Number of variables above that need to be accessed
		{
			//Update variables based on input
			HostChannelName					= fields[0].text;
			CommandFeedback_Info			= fields[1].text;
			CommandFeedback_BotEnters		= fields[2].text;
			CommandFeedback_BotLeaves		= fields[3].text;
			CommandFeedback_LoginNew		= fields[4].text;
			CommandFeedback_LoginReturning	= fields[5].text;

			//Update PlayerPrefs to new settings
			PlayerPrefs.SetString("HostChannelName", HostChannelName);
			PlayerPrefs.SetString("CommandFeedback_Info", CommandFeedback_Info);
			PlayerPrefs.SetString("CommandFeedback_BotEnters", CommandFeedback_BotEnters);
			PlayerPrefs.SetString("CommandFeedback_BotLeaves", CommandFeedback_BotLeaves);
			PlayerPrefs.SetString("CommandFeedback_LoginNew", CommandFeedback_LoginNew);
			PlayerPrefs.SetString("CommandFeedback_LoginReturning", CommandFeedback_LoginReturning);
		}
		else
		{
			Debug.Log("Field is missing from variable 'fields' in UserInputManager!");
		}
	}

	public static string GetParsedMessage(string message, OnChatCommandReceivedArgs e = null)
	{
		if (e != null) message = message.Replace("$USER$", e.Command.ChatMessage.Username);
		message = message.Replace("$CHANNEL$", UserInputManager.HostChannelName);
		return message;
	}
	public void ReconnectBot()
	{
		if (!TwitchClient.Instance.Client.IsConnected)
		{
			TwitchClient.Instance.ConnectBot();
		}
	}
	public void DisconnectBot()
	{
		if (TwitchClient.Instance.Client.IsConnected)
		{
			TwitchClient.Instance.Client.SendMessage(TwitchClient.Instance.Client.JoinedChannels[0], GetParsedMessage(CommandFeedback_BotLeaves));
			TwitchClient.Instance.Client.Disconnect();
		}
	}
}

