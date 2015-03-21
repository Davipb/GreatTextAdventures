using System;
using System.Collections.Generic;

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

		public override bool Do(string action)
		{
			// Must have arguments
			if (string.IsNullOrWhiteSpace(action))
			{
				Console.WriteLine("Which direction?");
				return false;
			}

			// Move to the desired location
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

			return false;
		}

		public override void Help()
		{
			Console.WriteLine("Move:");
			Console.WriteLine("\tmove *direction*");
			Console.WriteLine("\t\tdirection: Direction to move to. Can be: North/Up, South/Down, East/Right, or West/Left");
		}
	}
}
