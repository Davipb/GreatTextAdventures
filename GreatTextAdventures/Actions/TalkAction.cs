using GreatTextAdventures.People;
using System;
using System.Collections.Generic;

namespace GreatTextAdventures.Actions
{
	public class TalkAction : GameAction
	{
		public override IEnumerable<string> Aliases
		{
			get
			{
				yield return "talk";
				yield return "speak";
			}
		}

		public override bool Do(string action)
		{
			if (action.StartsWith("with"))
			{
				// Remove 'with' so we can accept more natural strings (talk with dude instead of talk dude)
				action = action.Substring(4).Trim();
			}

			if (string.IsNullOrWhiteSpace(action))
			{
				GameSystem.WriteLine("Talk with who?");
				return false;
			}

			// Find all people with 'action' code name
			ILookable found = GameSystem.GetLookableWithName(action);

			if (found == null) return false;

			Person person = found as Person;

			if (person == null)
			{
				GameSystem.WriteLine($"You can't talk with {found.DisplayName}");
				ListItemPossibilities(found);
				return false;
			}

			person.Talk();

			return true;
		}

		public override void Help()
		{
			GameSystem.WriteLine("Talk:");
			GameSystem.WriteLine("\ttalk *target*");
			GameSystem.WriteLine("\ttalk with *target*");
			GameSystem.WriteLine("\t\ttarget: Who to talk with");
		}
	}
}
