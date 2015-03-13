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
				// Show general help
				Console.WriteLine("Debug:");
				Console.WriteLine("\tdebug map [size] [file]");
			}
			else if (split[0] == "map")
			{
				if (split.Length != 3)
				{
					// Show 'map' help
					Console.WriteLine("Usage:");
					Console.WriteLine("\tdebug map [size] [file]");
					Console.WriteLine("\t\tsize: integer specifying the map radius, from 0;0");
					Console.WriteLine("\t\tfile: file to save the map to");
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
	}
}
