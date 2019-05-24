using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Commands
{
	public enum ADSCommand { Attack = 0, Defend = 1, Steal = 2 }
	public static readonly Dictionary<string, ADSCommand> ADSCommandDict = 
		new Dictionary<string, ADSCommand>()
		{
			{ "Attack", ADSCommand.Attack },
			{ "Defend", ADSCommand.Defend },
			{ "Steal", ADSCommand.Steal }
		};
	public enum GeneralCommand { EnterQueue }
}
