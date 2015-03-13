using System;
using System.Collections.Generic;

namespace GreatTextAdventures.Actions
{
	public class DebugAction : Action
	{
		public override IEnumerable<string> Aliases
		{
			get { yield return "debug"; }
		}

		public override void Do(string action)
		{
			// Arguments are obligatory
			if (string.IsNullOrWhiteSpace(action))
			{
				Console.WriteLine("Invalid command");
				return;
			}

			string[] split = action.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

			if (split.Length == 0)
			{
				// Show help
				Help();
			}
			else if (split[0] == "map")
			{
				if (split.Length != 3)
				{
					Console.WriteLine("Invalid number of arguments");
					return;
				}

				int size;
				
				if (int.TryParse(split[1], out size))
				{
					try
					{
						GameSystem.CurrentMap.Draw(size).Save(split[2]);
						Console.WriteLine("Map saved successfully");
					}
					catch (Exception e)
					{
						Console.WriteLine("Error saving map: {0}", e.Message);
					}
				}
				else
				{
					Console.WriteLine("Invalid size '{0}'", split[1]);
				}								
			}
		}

		public override void Help()
		{
			Console.WriteLine("Debug:");
			Console.WriteLine("\tdebug map *size* *file*");
			Console.WriteLine("\tsize: Radius, centered in 0;0, of the map to show");
			Console.WriteLine("\tfile: Path of the file where the map will be saved. Can be absolute or relative");
		}
	}
}
