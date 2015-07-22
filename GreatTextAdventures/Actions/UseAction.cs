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
				GameSystem.WriteLine("Use what?");
				return false;
			}

			ILookable found = GameSystem.GetLookableWithName(action);

			if (found == null) return false;

			IUsable item = found as IUsable;

			if (item == null)
			{
				GameSystem.WriteLine($"You can't use '{found.DisplayName}'");
				ListItemPossibilities(found);
				return false;
			}

			item.Use(GameSystem.Player);

			return true;
		}

		public override void Help()
		{
			GameSystem.WriteLine("Use:");
			GameSystem.WriteLine("\tuse*target*");
			GameSystem.WriteLine("\t\ttarget: What to use");
		}
	}
}
