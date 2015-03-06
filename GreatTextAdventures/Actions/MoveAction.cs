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
			}
		}

		public override void Do(string action)
		{
			switch (action)
			{
				case "up":
				case "north":
					System.CurrentMap.Move(Directions.North);
					break;
				case "down":
				case "south":
					System.CurrentMap.Move(Directions.South);
					break;
				case "right":
				case "east":
					System.CurrentMap.Move(Directions.East);
					break;
				case "left":
				case "west":
					System.CurrentMap.Move(Directions.West);
					break;
				default:
					Console.WriteLine("Unknown direction '{0}'", action);
					break;
			}
		}
	}
}
