﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace GreatTextAdventures.Actions
{
	public class LookAction : Action
	{
		public override IEnumerable<string> Aliases
		{
			get 
			{ 
				yield return "look";
				yield return "examine";
				yield return "analyze";
			}
		}

		public override void Do(string action)
		{
			if (action.StartsWith("at"))
			{
				// Remove 'at', so we can accept a more natural speech style (look at stuff, instead of look stuff)
				action = action.Substring(2).Trim();
			}

			if (string.IsNullOrWhiteSpace(action) || action == "around" || action == "room")
			{
				// Just look at the room, in general
				Console.WriteLine("You are in a {0}", GameSystem.CurrentMap.CurrentRoom.Describe());
			}
			else
			{
				// Get all the items or creatures with the codename 'action'
				IList<ILookable> found = GameSystem.CurrentMap.CurrentRoom.Members.Where(x => x.CodeNames.Contains(action)).ToList();

				if (found.Count == 0)
				{
					Console.WriteLine("There is no '{0}'.", action);
				}
				else if (found.Count > 1)
				{
					ILookable chosen = GameSystem.Choice<ILookable>(found, found.Select(x => x.DisplayName).ToList());
					Console.WriteLine(chosen.Description);
				}
				else
				{
					Console.WriteLine(found[0].Description);
				}
			}			
		}

		public override void Help()
		{
			Console.WriteLine("Look:");
			Console.WriteLine("\tlook *target*");
			Console.WriteLine("\tlook at *target*");
			Console.WriteLine("\t\ttarget: Who or What to look at");
		}
	}
}