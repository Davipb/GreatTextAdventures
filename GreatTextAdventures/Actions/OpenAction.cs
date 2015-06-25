using GreatTextAdventures.Items;
using System;
using System.Collections.Generic;

namespace GreatTextAdventures.Actions
{
	public class OpenAction : GameAction
	{
		public override IEnumerable<string> Aliases
		{
			get { yield return "open"; }
		}

		public override bool Do(string action)
		{
			if (string.IsNullOrWhiteSpace(action))
			{
				GameSystem.WriteLine("Open what?");
				return false;
			}

			ILookable found = GameSystem.GetLookableWithName(action);

			if (found == null) return false;

			IContainer container = found as IContainer;

			if (container == null)
			{
				GameSystem.WriteLine("You can't open '{0}'", found.DisplayName);
				ListItemPossibilities(found);
				return false;
			}

			container.Open();

			return true;
		}

		public override void Help()
		{
			GameSystem.WriteLine("Open:");
			GameSystem.WriteLine("\topen *target*");
			GameSystem.WriteLine("\t\ttarget: What to open");
		}
	}
}
