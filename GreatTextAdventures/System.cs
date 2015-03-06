using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatTextAdventures
{
	public static class System
	{
		public static List<Action> Actions { get; set; }
		public static Map CurrentMap { get; set; }
		public static Random RNG { get; set; }

		public static void Initialize()
		{
			RNG = new Random();

			Actions = new List<Action>();
			Actions.Add(new Actions.LookAction());
			Actions.Add(new Actions.MoveAction());

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
	}
}
