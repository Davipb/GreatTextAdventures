using System;
using System.Collections.Generic;
using System.Linq;

using GreatTextAdventures.People;
using GreatTextAdventures.Actions;
using GreatTextAdventures.Items;
using GreatTextAdventures.Rooms;

namespace GreatTextAdventures
{
	public static class GameSystem
	{
		public static List<GameAction> Actions { get; set; }
		public static Map CurrentMap { get; set; }
		public static Random RNG { get; set; }
		public static Person Player { get; set; }

		static void Main(string[] args)
		{
			Console.Title = "Great Text Adventures";
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

			Actions = new List<GameAction>();
			Actions.Add(new LookAction());
			Actions.Add(new MoveAction());
			Actions.Add(new DebugAction());
			Actions.Add(new TalkAction());
			Actions.Add(new OpenAction());
			Actions.Add(new HelpAction());
			Actions.Add(new EquipAction());
			Actions.Add(new AttackAction());
			Actions.Add(new WaitAction());
			Actions.Add(new CastAction());

			Player = new PlayerPerson();

			CurrentMap = new Map();			
		}

		/// <summary>
		/// Performs the main game loop
		/// </summary>
		public static void Loop()
		{
			Console.WriteLine(Player.Description);

			while(true)
			{
				// Beautification
				Console.WriteLine();
				Console.Write("> ");

				// Standardize all input to trimmed lowercase
				string input = Console.ReadLine().Trim().ToLowerInvariant();

				// Beautification
				Console.WriteLine();

				if (string.IsNullOrWhiteSpace(input)) continue;

				// Flag to know whether or not the user entered a correct input
				bool didAction = false;

				// Search for valid actions
				foreach(var act in Actions)
				{
					foreach(var alias in act.Aliases)
					{
						if (input.StartsWith(alias))
						{
							// If the action allows it, update the game
							if (act.Do(input.Substring(alias.Length).Trim()))
							{
								CurrentMap.Update();
								Player.Update();
							}
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
			
				// Check for player death
				if (Player.Health <= 0)
				{
					Console.WriteLine();
					Console.WriteLine("*****YOU HAVE DIED*****");
					Console.WriteLine();
					Console.WriteLine("Press any key to exit");
					Console.ReadKey();
					break;
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

			if (Player.CodeNames.Contains(name)) found.Add(Player);

			if (found.Count == 0)
			{
				Console.WriteLine("There is no '{0}'", name);
				return null;
			}
			else if (found.Count > 1)
			{
				Console.WriteLine("There are multiple '{0}'. Please specify.", name);
				return Choice<ILookable>(found, found.Select(x => x.DisplayName).ToList());
			}
			else
			{
				return found[0];
			}
		}
	}
}
