using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
			Console.WriteLine("{0} waits", GameSystem.Player.DisplayName);
			return true;
		}

		public override void Help()
		{
			Console.WriteLine("Wait:");
			Console.WriteLine("\twait");
		}
	}
}
