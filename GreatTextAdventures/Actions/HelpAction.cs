using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GreatTextAdventures.Actions
{
	public class HelpAction : GameAction
	{
		public override IEnumerable<string> Aliases
		{
			get { yield return "help"; }
		}

		public override bool Do(string action)
		{
			if (string.IsNullOrEmpty(action))
			{
				Console.WriteLine("List of actions, grouped per line (actions in the same group have the same function):");
				foreach(GameAction a in GameSystem.Actions)
				{
					StringBuilder sb = new StringBuilder();

					foreach(string alias in a.Aliases)
					{
						sb.Append(alias);
						sb.Append(", ");
					}

					sb.Remove(sb.Length - 2, 2);
					Console.WriteLine(sb);
				}

				return false;
			}

			foreach(GameAction a in GameSystem.Actions)
			{
				if (a.Aliases.Contains(action))
				{
					a.Help();
					return false;
				}
			}

			Console.WriteLine("Unknown action '{0}'. Type only 'help' for a list of actions.", action);

			return false;
		}

		public override void Help()
		{
			Console.WriteLine("Help:");
			Console.WriteLine("\thelp *action*");
			Console.WriteLine("\t\taction: GameAction to get help with");
		}
	}
}
