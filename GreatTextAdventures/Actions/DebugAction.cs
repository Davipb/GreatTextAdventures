using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

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
			string[] split = action.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

			if (split.Length == 0)
			{
				// Show help
				Console.WriteLine("Debug:");
				Console.WriteLine("\tdebug map [size] [file]");
				Console.WriteLine("\t\tsize: integer specifying the map radius, from 0;0");
				Console.WriteLine("\t\tfile: file to save the map to");
			}
			else if (split[0] == "map")
			{
				if (split.Length != 3)
				{
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
