using System;
using System.Collections.Generic;
using System.Linq;

namespace GreatTextAdventures.Actions
{
	public class LookAction : GameAction
	{
		public override IEnumerable<string> Aliases
		{
			get 
			{ 
				yield return "look";
				yield return "examine";
				yield return "analyze";
			}
		}

		public override bool Do(string action)
		{
			if (action.StartsWith("at"))
			{
				// Remove 'at', so we can accept a more natural speech style (look at stuff, instead of look stuff)
				action = action.Substring(2).Trim();
			}

			if (string.IsNullOrWhiteSpace(action) || action == "around" || action == "room")
			{
				// Just look at the room, in general
				Console.WriteLine(GameSystem.CurrentMap.CurrentRoom.Describe());
			}
			else
			{
				ILookable found = GameSystem.GetMemberWithName(action);

				// Exit if input is invalid (nothing found)
				if (found == null) return false;

				Console.WriteLine(found.Description);
			}

			return false;
		}

		public override void Help()
		{
			Console.WriteLine("Look:");
			Console.WriteLine("\tlook *target*");
			Console.WriteLine("\tlook at *target*");
			Console.WriteLine("\t\ttarget: Who or What to look at");
		}
	}
}
