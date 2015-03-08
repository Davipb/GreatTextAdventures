using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatTextAdventures
{
	public static class GameSystem
	{
		public static List<Action> Actions { get; set; }
		public static Map CurrentMap { get; set; }
		public static Random RNG { get; set; }
		public static Person Player { get; set; }

		public static void Initialize()
		{
			RNG = new Random();

			Actions = new List<Action>();
			Actions.Add(new Actions.LookAction());
			Actions.Add(new Actions.MoveAction());
			Actions.Add(new Actions.DebugAction());

			CurrentMap = new Map();			
		}

		public static void Loop()
		{
			while(true)
			{
				// Beautification
				Console.WriteLine();
				Console.Write("> ");

				string input = Console.ReadLine().Trim().ToLowerInvariant();

				// Beautification
				Console.WriteLine();

				// Flag to know whether or not the user entered a correct input
				bool didAction = false;

				// Search for valid actions
				foreach(var act in Actions)
				{
					foreach(var alias in act.Aliases)
					{
						if (input.StartsWith(alias))
						{
							// If there's a valid action, invoke it and set the flag
							act.Do(input.Substring(alias.Length).Trim());
							didAction = true;
							break;
						}
					}

					if (didAction) break;
				}

				// User entered an invalid input
				if (!didAction)
				{
					Console.WriteLine("Unknown action '{0}'", input.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries)[0]);
				}				
			}
		}

		public static T Choice<T>(IList<T> items, IList<string> displayNames = null)
		{
			if (items == null) throw new ArgumentNullException("items");
			if (displayNames == null) displayNames = items.Select(x => x.ToString()).ToList();

			int page = 0;
			int maxPages = items.Count / 8;

			while (true)
			{
				Console.Clear();

				for (int i = 0; i < 8; i++)
				{
					// Prevent 'Index out of Bounds'
					if (items.Count <= (page * 8) + i) break;

					Console.WriteLine("{0}. {1}", i % 9 + 1, displayNames[(page * 8) + i]);
				}

				if (page > 0) Console.WriteLine("9. Previous Page");
				if (page < maxPages) Console.WriteLine("0. Next Page");

				int selected;

				if (int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out selected))
				{
					if (selected == 0 && page < maxPages)
					{
						page++;
						continue;
					}
					else if (selected == 9 && page > 0)
					{
						page--;
						continue;
					}
					else if (selected > 0 && selected < 9)
					{
						if (page != maxPages)
						{
							return items[(page * 8) + selected - 1];
						}
						else if (selected < items.Count % 8)
						{
							return items[(page * 8) + selected - 1];
						}
					}
				}
			}
		}
	}
}
