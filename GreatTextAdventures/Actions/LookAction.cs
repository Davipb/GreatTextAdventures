using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatTextAdventures.Actions
{
	public class LookAction : Action
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

		public override void Do(string action)
		{
			if (action.StartsWith("at"))
			{
				// Remove 'at', so we can accept a more natural speech style (look at stuff, instead of look stuff)
				action = action.Substring(2).Trim();
			}

			if (string.IsNullOrWhiteSpace(action) || action == "around" || action == "room")
			{
				// Just look at the room, in general
				Console.WriteLine("You are in a {0}", GameSystem.CurrentMap.CurrentRoom.Describe());
			}
			else
			{
				// Get all the items or creatures with the codename 'action'
				IList<ILookable> found = GameSystem.CurrentMap.CurrentRoom.Members.Where(x => x.CodeNames.Contains(action)).ToList();

				if (found.Count == 0)
				{
					Console.WriteLine("There is no '{0}'.", action);
				}
				else if (found.Count > 1)
				{
					ILookable chosen = GameSystem.Choice<ILookable>(found, found.Select(x => x.DisplayName).ToList());
					Console.Clear();

					Console.WriteLine(chosen.Description);
				}
				else
				{
					Console.WriteLine(found[0].Description);
				}
			}
			
		}
	}
}
