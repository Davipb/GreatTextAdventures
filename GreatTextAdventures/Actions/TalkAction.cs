using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
				IList<Person> found = GameSystem.CurrentMap.CurrentRoom.Members.Where(x => x is Person && x.CodeNames.Contains(action)).Select(x => (Person)x).ToList();

				if (found.Count == 0)
				{
					Console.WriteLine("There is no '{0}'", action);
				}
				else if (found.Count > 1)
				{
					Console.WriteLine("There are multiple '{0}'. Please specify:");
					Person chosen = GameSystem.Choice<Person>(found, found.Select(x => x.DisplayName).ToList());
					chosen.Talk();
				}
				else
				{
					found[0].Talk();
				}
			}
		}
	}
}
