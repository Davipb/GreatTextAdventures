using System;
using System.Collections.Generic;
using System.Linq;

namespace GreatTextAdventures
{
	public static class GameSystem
	{
		public static List<Action> Actions { get; set; }
		public static Map CurrentMap { get; set; }
		public static Random RNG { get; set; }
		public static Person Player { get; set; }

		static void Main(string[] args)
		{
			GameSystem.Initialize();
			GameSystem.Loop();
		}

		public static void Initialize()
		{
			RNG = new Random();

			Actions = new List<Action>();
			Actions.Add(new Actions.LookAction());
			Actions.Add(new Actions.MoveAction());
			Actions.Add(new Actions.DebugAction());
			Actions.Add(new Actions.TalkAction());
			Actions.Add(new Actions.OpenAction());
			Actions.Add(new Actions.HelpAction());

			CurrentMap = new Map();			
		}

		/// <summary>
		/// Performs the main game loop
		/// </summary>
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
					Console.WriteLine("Unknown action '{0}'. Type 'help' for a list of actions.", input.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries)[0]);
				}				
			}
		}

		/// <summary>
		/// Asks the user to choose one item out of a list
		/// </summary>
		/// <typeparam name="T">The type of items the list contains</typeparam>
		/// <param name="items">The list to choose from</param>
		/// <param name="displayNames">The names to present the user with when choosing</param>
		/// <returns>The item chosen by the user</returns>
		public static T Choice<T>(IList<T> items, IList<string> displayNames = null)
		{
			// Must have items list
			if (items == null) throw new ArgumentNullException("items");
			// If no display list is provided, default to "ToString" representation
			if (displayNames == null) displayNames = items.Select(x => x.ToString()).ToList();

			// Display all items to the user
			for (int i = 0; i < items.Count; i++ )
			{
				Console.WriteLine("{0}. {1}", i + 1, displayNames[i]);
			}

			Console.WriteLine();
			Console.WriteLine("Write input:");

			while(true)
			{
				string input = Console.ReadLine();
				int output;

				if (int.TryParse(input, out output) && output > 0 && output <= items.Count)
				{
					return items[output - 1];
				}
				else
				{
					// Clear the user input, clearing the whole line
					int currentLineCursor = Console.CursorTop - 1;
					Console.SetCursorPosition(0, Console.CursorTop - 1);
					Console.Write(new string(' ', Console.WindowWidth));
					Console.SetCursorPosition(0, currentLineCursor);
				}
			}
		}
	}
}
