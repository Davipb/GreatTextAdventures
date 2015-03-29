using GreatTextAdventures.Items;
using System;
using System.Collections.Generic;

namespace GreatTextAdventures.Actions
{
	public class UseAction : GameAction
	{
		public override IEnumerable<string> Aliases
		{
			get 
			{
				yield return "use";
			}
		}

		public override bool Do(string action)
		{
			if (string.IsNullOrWhiteSpace(action))
			{
				Console.WriteLine("Use what?");
				return false;
			}

			ILookable found = GameSystem.GetMemberWithName(action);

			if (found == null) return false;

			IUsable item = found as IUsable;

			if (item == null)
			{
				Console.WriteLine("You can't use '{0}'", found.DisplayName);
				return false;
			}

			item.Use(GameSystem.Player);

			return true;
		}

		public override void Help()
		{
			Console.WriteLine("Use:");
			Console.WriteLine("\tuse*target*");
			Console.WriteLine("\t\ttarget: What to use");
		}
	}
}
