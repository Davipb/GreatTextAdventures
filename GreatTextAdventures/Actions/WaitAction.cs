using System;
using System.Collections.Generic;

namespace GreatTextAdventures.Actions
{
	public class WaitAction : GameAction
	{
		public override IEnumerable<string> Aliases
		{
			get 
			{
				yield return "wait";
			}
		}

		public override bool Do(string action)
		{
			GameSystem.WriteLine("{0} waits", GameSystem.Player.DisplayName);
			return true;
		}

		public override void Help()
		{
			GameSystem.WriteLine("Wait:");
			GameSystem.WriteLine("\twait");
		}
	}
}
