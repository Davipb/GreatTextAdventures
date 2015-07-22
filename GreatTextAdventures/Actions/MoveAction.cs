using System;
using System.Collections.Generic;

namespace GreatTextAdventures.Actions
{
	public class MoveAction : GameAction
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
				GameSystem.WriteLine("Which direction?");
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
					GameSystem.WriteLine($"Unknown direction '{action}'");
					return false;
			}

			return true;
		}

		public override void Help()
		{
			GameSystem.WriteLine("Move:");
			GameSystem.WriteLine("\tmove *direction*");
			GameSystem.WriteLine("\t\tdirection: Direction to move to. Can be: North/Up, South/Down, East/Right, or West/Left");
		}
	}
}
