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
#if DEBUG
			GameSystem.Initialize();
			GameSystem.Loop();
#else
			try
			{
				GameSystem.Initialize();
				GameSystem.Loop();
			}
			catch (Exception e)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine();
				Console.WriteLine("Unexpected error!");
				Console.WriteLine(e.Message);
				Console.WriteLine();
				Console.WriteLine("Press s to show Stack Trace or any other key to exit");
				if (char.ToLower(Console.ReadKey(true).KeyChar) == 's')
				{
					Console.WriteLine(e.StackTrace);
					Console.ReadKey();
				}
			}
#endif
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
			Actions.Add(new Actions.EquipAction());

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

		public static ILookable GetMemberWithName(string name)
		{
			IList<ILookable> found = (from item in CurrentMap.CurrentRoom.Members
									  where item.CodeNames.Contains(name)
									  select item)
									 .ToList();

			if (found.Count == 0)
			{
				Console.WriteLine("There is no '{0}'", name);
				return null;
			}
			else if (found.Count > 1)
			{
				Console.WriteLine("There are multiple '{0}'. Please specify.");
				return Choice<ILookable>(found, found.Select(x => x.DisplayName).ToList());
			}
			else
			{
				return found[0];
			}
		}
	}
}
