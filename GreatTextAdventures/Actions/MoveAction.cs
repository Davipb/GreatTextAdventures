using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatTextAdventures.Actions
{
	public class MoveAction : Action
	{
		public override IEnumerable<string> Aliases
		{
			get 
			{
				yield return "move";
				yield return "leave";
				yield return "walk";
				yield return "run";
				yield return "go";
				yield return "enter";
			}
		}

		public override void Do(string action)
		{
			if (string.IsNullOrWhiteSpace(action))
			{
				Console.WriteLine("Which direction?");
				return;
			}

			switch (action)
			{
				case "up":
				case "north":
					GameSystem.CurrentMap.Move(Directions.North);
					break;
				case "down":
				case "south":
					GameSystem.CurrentMap.Move(Directions.South);
					break;
				case "right":
				case "east":
					GameSystem.CurrentMap.Move(Directions.East);
					break;
				case "left":
				case "west":
					GameSystem.CurrentMap.Move(Directions.West);
					break;
				default:
					Console.WriteLine("Unknown direction '{0}'", action);
					break;
			}
		}
	}
}
