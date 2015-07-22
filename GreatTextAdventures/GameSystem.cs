using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GreatTextAdventures.People;
using GreatTextAdventures.Actions;
using GreatTextAdventures.Items;
using GreatTextAdventures.Rooms;

namespace GreatTextAdventures
{
	/// <summary>
	/// Main class that contains the essential game objects and helper methods
	/// </summary>
	public static class GameSystem
	{
		// These two are used in Write/WriteLine
		const char SpecialFunctionChar = '&';
		const ConsoleColor DefaultConsoleColor = ConsoleColor.Gray;

		public static List<GameAction> Actions { get; set; }
		public static Map CurrentMap { get; set; }
		public static Random RNG { get; set; }
		public static PlayerPerson Player { get; set; }

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

		/// <summary>
		/// Initializes the default values for the game
		/// </summary>
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
			Actions.Add(new UseAction());
			Actions.Add(new LevelAction());
			Actions.Add(new TakeAction());
			Actions.Add(new DropAction());

			Player = new PlayerPerson();

			CurrentMap = new Map();
		}

		/// <summary>
		/// Performs the main game loop
		/// </summary>
		public static void Loop()
		{
			GameSystem.WriteLine();
			GameSystem.WriteLine(Player.Description);

			CurrentMap.Update();
			Player.Update();

			while (true)
			{
				// Indicates the user can write their command with a ">" at the start of the line
				GameSystem.WriteLine();
				GameSystem.Write("> ");

				// Read and standardize all input to trimmed lowercase
				string input = Console.ReadLine().Trim().ToLowerInvariant();

				// Beautification
				GameSystem.WriteLine();

				if (string.IsNullOrWhiteSpace(input)) continue;

				bool didAction = false;

				// Search for valid actions
				foreach (var act in Actions)
				{
					foreach (var alias in act.Aliases)
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
					GameSystem.WriteLine("Unknown action '{0}'. Type 'help' for a list of actions.", input.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries)[0]);
				}

				// Check for player death
				if (Player.Health <= 0)
				{
					GameSystem.WriteLine();
					GameSystem.WriteLine("*****YOU HAVE DIED*****");
					GameSystem.WriteLine();
					GameSystem.WriteLine("Press any key to exit");
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
			if (displayNames != null)
				return Choice<T>(items.Zip(displayNames, (x, y) => Tuple.Create<T, string>(x, y)).ToList());
			else
				return Choice<T>(items.Select(x => Tuple.Create(x, x.ToString())).ToList());
		}

		/// <summary>
		/// Asks the user to choose one item out of a list
		/// </summary>
		/// <typeparam name="T">The type of items the list contains</typeparam>
		/// <param name="items">The list of items with their displayed names</param>
		/// <returns>The item chosen by the user</returns>
		public static T Choice<T>(IList<Tuple<T, string>> items)
		{
			// Must have items list
			if (items == null) throw new ArgumentNullException("items");

			// Display all items to the user
			for (int i = 0; i < items.Count; i++)
			{
				GameSystem.WriteLine($"{i + 1}. {items[i].Item2}");
			}

			GameSystem.WriteLine();
			GameSystem.WriteLine("Write input:");

			while (true)
			{
				string input = Console.ReadLine();
				int output;

				if (int.TryParse(input, out output) && output > 0 && output <= items.Count)
				{
					return items[output - 1].Item1;
				}
				else
				{
					// Clear the user input, clearing the whole line
					int currentLineCursor = Console.CursorTop - 1;
					Console.SetCursorPosition(0, Console.CursorTop - 1);
					GameSystem.Write(new string(' ', Console.WindowWidth));
					Console.SetCursorPosition(0, currentLineCursor);
				}
			}
		}

		/// <summary>
		/// Finds an ILookable with the required name in the current Room or in the Player's inventory, asking the user if more than 1 item is found
		/// </summary>
		/// <param name="name">The name of the ILookable to search for</param>
		/// <returns>The found ILookable or null if nothing was found</returns>
		public static ILookable GetLookableWithName(string name)
		{
			// Find all applicable items then assign a name to be shown if multiple items are found
			// 'name' can either be a CodeName or the full DisplayName

			var inRoom = from item in CurrentMap.CurrentRoom.Members
						 where item.CodeNames.Contains(name) || item.DisplayName.ToLowerInvariant() == name
						 select Tuple.Create(item, item.DisplayName + " [Room]");

			var inInventory = from item in Player.Inventory
							  where item.CodeNames.Contains(name) || item.DisplayName.ToLowerInvariant() == name
							  select Tuple.Create(item, item.DisplayName + " [Inventory]");

			var found = inRoom.Union(inInventory).ToList();

			if (Player.EquippedWeapon.CodeNames.Contains(name) || Player.EquippedWeapon.DisplayName.ToLowerInvariant() == name)
				found.Add(Tuple.Create<ILookable, string>(Player.EquippedWeapon, Player.EquippedWeapon.DisplayName + " [Inventory, Equipped]"));

			if (Player.CodeNames.Contains(name) || Player.DisplayName.ToLowerInvariant() == name)
				found.Add(Tuple.Create<ILookable, string>(Player, Player.DisplayName + " [You]"));

			if (found.Count == 0)
			{
				GameSystem.WriteLine($"There is no '{name}'");
				return null;
			}
			else if (found.Count > 1)
			{
				GameSystem.WriteLine($"There are multiple '{name}'. Please specify.");
				return Choice<ILookable>(found);
			}
			else
			{
				return found[0].Item1;
			}
		}

		/// <summary>
		/// Formats a list a items into a 'list string'
		/// </summary>
		/// <typeparam name="T">The type of the items in the list</typeparam>
		/// <param name="list">The list of items to enumerate</param>
		/// <param name="multiPrefix">String that goes before the items when there are 2 or more of them</param>
		/// <param name="onePrefix">String that goes before the item when there is only one of them</param>
		/// <param name="nonePrefix">String to show when there are no items in the list</param>
		/// <param name="lastSeparator">String that goes before the last item in the enumeration ('and', 'or', etc)</param>
		/// <returns>The formatted enumeration</returns>
		public static string Enumerate<T>(IEnumerable<T> list, string multiPrefix, string onePrefix, string nonePrefix, string lastSeparator)
		{
			StringBuilder sb = new StringBuilder();

			if (list.Count() > 2)
			{
				// Put items in format "<multiPrefix> a, b, c, ..., d, <lastSeparator> e"
				sb.Append(multiPrefix);
				sb.Append(" ");

				sb.AppendFormat($"{list.First().ToString()},");

				foreach (T elem in list.Skip(1).Take(list.Count() - 2))
					sb.AppendFormat($" {elem.ToString()},");

				sb.AppendFormat($" {lastSeparator} {list.Last().ToString()}");
			}
			else if (list.Count() == 2)
			{
				// Two items, put them in format "<multiPrefix> a <lastSeparator> b"
				sb.AppendFormat($"{multiPrefix} {list.First()} {lastSeparator} {list.Last()}");
			}
			else if (list.Count() > 0)
			{
				// One item, put it in format "<onePrefix/multiPrefix> a"
				sb.AppendFormat($"{onePrefix ?? multiPrefix} {list.First()}");
			}
			else
			{
				// No items, put it in format "<nonePrefix/multiPrefix>"
				sb.Append(nonePrefix ?? multiPrefix);
			}

			return sb.ToString();
		}

		/// <summary>
		/// Writes a certain text to the console, formatting it with certain arguments. Supports special-character functions.
		/// </summary>
		/// <param name="text">Text to write</param>
		/// <param name="args">Arguments to use when formatting the text (see String.Format)</param>
		public static void Write(string text, params object[] args) => 
			WriteColor(string.Format(text, args), 0, DefaultConsoleColor);


		/// <summary>
		/// Writes an object to the console.
		/// </summary>
		/// <param name="obj">Object to write</param>
		public static void Write(object obj) =>
			Write(obj.ToString());

		private static int WriteColor(string text, int startingIndex, ConsoleColor color)
		{
			for (int i = startingIndex; i < text.Length; i++)
			{
				Console.ForegroundColor = color;

				if (text[i] != SpecialFunctionChar)
				{
					Console.Write(text[i]);
					continue;
				}

				// 'Color' function -> &CXX, where XX is a double-digit color code or 'EE' to indicate this 'color level' has ended
				if (char.ToUpperInvariant(text[++i]) == 'C')
				{
					string param = new string(new[] { text[++i], text[++i] });

					if (param.ToUpperInvariant() == "EE")
					{
						return i;
					}
					else
					{
						// Go down a 'color level', writing all input in the specified color until &CEE is found, then go up a 'color level',
						// returning to the color specified in this method
						int index;

						if (int.TryParse(param, out index))
						{
							ConsoleColor newColor = (ConsoleColor)Enum.GetValues(typeof(ConsoleColor)).GetValue(index);
							i = WriteColor(text, ++i, newColor);
						}
						else
							Console.Write("ERR");
					}
				}
				else
				{
					Console.Write("ERR");
				}
			}

			return text.Length - 1;
		}

		/// <summary>
		/// Writes a certain text to the console, formatting it with certain arguments, then prints a line. Supports special-character functions.
		/// </summary>
		/// <param name="text">Text to write</param>
		/// <param name="args">Arguments to use when formatting the text (see String.Format)</param>
		public static void WriteLine(string text, params object[] args)
		{
			Write(text, args);
			Console.WriteLine();
		}

		/// <summary>
		/// Writes an object to the console, then prints a line.
		/// </summary>
		/// <param name="obj">Object to write</param>
		public static void WriteLine(object obj) =>
			WriteLine(obj.ToString());

		/// <summary>
		/// Prints a line.
		/// </summary>
		public static void WriteLine() =>
			Console.WriteLine();

	}
}
