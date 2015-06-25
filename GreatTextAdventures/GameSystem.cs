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
	public static class GameSystem
	{
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
				GameSystem.WriteLine();
				GameSystem.WriteLine("Unexpected error!");
				GameSystem.WriteLine(e.Message);
				GameSystem.WriteLine();
				GameSystem.WriteLine("Press s to show Stack Trace or any other key to exit");
				if (char.ToLower(Console.ReadKey(true).KeyChar) == 's')
				{
					GameSystem.WriteLine(e.StackTrace);
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
			Actions.Add(new UseAction());
			Actions.Add(new LevelAction());

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
				// Beautification
				GameSystem.WriteLine();
				GameSystem.Write("> ");

				// Standardize all input to trimmed lowercase
				string input = Console.ReadLine().Trim().ToLowerInvariant();

				// Beautification
				GameSystem.WriteLine();

				if (string.IsNullOrWhiteSpace(input)) continue;

				// Flag to know whether or not the user entered a correct input
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
			// Must have items list
			if (items == null) throw new ArgumentNullException("items");
			// If no display list is provided, default to "ToString" representation
			if (displayNames == null) displayNames = items.Select(x => x.ToString()).ToList();

			// Display all items to the user
			for (int i = 0; i < items.Count; i++)
			{
				GameSystem.WriteLine("{0}. {1}", i + 1, displayNames[i]);
			}

			GameSystem.WriteLine();
			GameSystem.WriteLine("Write input:");

			while (true)
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
					GameSystem.Write(new string(' ', Console.WindowWidth));
					Console.SetCursorPosition(0, currentLineCursor);
				}
			}
		}

		public static ILookable GetMemberWithName(string name)
		{
			IList<ILookable> found = (from item in CurrentMap.CurrentRoom.Members
									  where item.CodeNames.Contains(name) || item.DisplayName.ToLowerInvariant() == name
									  select item)
									 .ToList();

			if (Player.CodeNames.Contains(name)) found.Add(Player);

			if (found.Count == 0)
			{
				GameSystem.WriteLine("There is no '{0}'", name);
				return null;
			}
			else if (found.Count > 1)
			{
				GameSystem.WriteLine("There are multiple '{0}'. Please specify.", name);
				return Choice<ILookable>(found, found.Select(x => x.DisplayName).ToList());
			}
			else
			{
				return found[0];
			}
		}

		public static string Enumerate<T>(IEnumerable<T> list, string multiPrefix, string onePrefix, string nonePrefix, string lastSeparator)
		{
			StringBuilder sb = new StringBuilder();

			if (list.Count() > 2)
			{
				// Put items in format "<multiPrefix> a, b, c, ..., d, <lastSeparator> e"
				sb.Append(multiPrefix);
				sb.Append(" ");

				sb.AppendFormat("{0},", list.First().ToString());

				foreach (T elem in list.Skip(1).Take(list.Count() - 2))
					sb.AppendFormat(" {0},", elem.ToString());

				sb.AppendFormat(" {0} {1}", lastSeparator, list.Last().ToString());
			}
			else if (list.Count() == 2)
			{
				// Dual items, put them in format "<multiPrefix> a <lastSeparator> b"
				sb.AppendFormat("{0} {1} {2} {3}", multiPrefix, list.First(), lastSeparator, list.Last());
			}
			else if (list.Count() > 0)
			{
				// One item, put it in format "<onePrefix/multiPrefix> a"
				sb.AppendFormat("{0} {1}", onePrefix ?? multiPrefix, list.First());
			}
			else
			{
				// No items, put it in format "<nonePrefix/multiPrefix>"
				sb.Append(nonePrefix ?? multiPrefix);
			}

			return sb.ToString();
		}

		public static void Write(string text, params object[] args)
		{
			string s = string.Format(text, args);

			WriteColor(s, 0, DefaultConsoleColor);
		}

		public static void Write(object obj)
		{
			Write(obj.ToString());
		}

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

				if (char.ToUpperInvariant(text[++i]) == 'C')
				{
					string param = new string(new[] { text[++i], text[++i] });

					if (param.ToUpperInvariant() == "EE")
					{
						return i;
					}
					else
					{
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

		public static void WriteLine(string text, params object[] args)
		{
			Write(text, args);
			Console.WriteLine();
		}

		public static void WriteLine(object obj)
		{
			WriteLine(obj.ToString());
		}

		public static void WriteLine()
		{
			Console.WriteLine();
		}


	}
}
