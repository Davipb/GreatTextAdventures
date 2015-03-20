using System;
using System.Collections.Generic;
using System.Linq;

namespace GreatTextAdventures.Actions
{
	public class TalkAction : Action
	{
		public override IEnumerable<string> Aliases
		{
			get 
			{ 
				yield return "talk";
				yield return "speak";
			}
		}

		public override void Do(string action)
		{
			if (action.StartsWith("with"))
			{
				// Remove 'with' so we can accept more natural strings (talk with dude instead of talk dude)
				action = action.Substring(4).Trim();
			}

			if (string.IsNullOrWhiteSpace(action))
			{
				Console.WriteLine("Talk with who?");
			}
			else
			{
				// Find all people with 'action' code name
				ILookable found = GameSystem.GetMemberWithName(action);

				if (found == null) return;

				Person person = found as Person;

				if (person == null)
				{
					Console.WriteLine("You can't talk with {0}", found.DisplayName);
					return;
				}

				person.Talk();
			}
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
