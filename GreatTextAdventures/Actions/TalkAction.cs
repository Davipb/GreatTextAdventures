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
				Console.WriteLine("Talk with who?");
				return false;
			}

			// Find all people with 'action' code name
			ILookable found = GameSystem.GetMemberWithName(action);

			if (found == null) return false;

			Person person = found as Person;

			if (person == null)
			{
				Console.WriteLine("You can't talk with {0}", found.DisplayName);
				return false;
			}

			person.Talk();

			return true;
		}

		public override void Help()
		{
			Console.WriteLine("Talk:");
			Console.WriteLine("\ttalk *target*");
			Console.WriteLine("\ttalk with *target*");
			Console.WriteLine("\t\ttarget: Who to talk with");
		}
	}
}
