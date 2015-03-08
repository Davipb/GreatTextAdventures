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
			get { yield return "look"; }
		}

		public override void Do(string action)
		{
			if (action.StartsWith("at"))
			{
				action = action.Substring(2).Trim();
			}

			if (string.IsNullOrWhiteSpace(action) || action == "around" || action == "room")
			{
				// Just look at the room, in general
				Console.WriteLine("You are in a {0}.", GameSystem.CurrentMap.CurrentRoom.Describe());
			}
			else
			{
				bool found = false;

				foreach (var item in GameSystem.CurrentMap.CurrentRoom.Items)
				{
					if (item.CodeNames.Contains(action))
					{
						Console.WriteLine(item.Description);
						found = true;
						break;
					}
				}

				if (!found)
				{
					Console.WriteLine("There is no '{0}'.", action);
				}
			}
			
		}
	}
}
