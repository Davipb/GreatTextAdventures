using System;
using System.Collections.Generic;
using System.Linq;

namespace GreatTextAdventures.Actions
{
	public class LookAction : GameAction
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

		public override bool Do(string action)
		{
			if (action.StartsWith("at "))
			{
				// Remove 'at', so we can accept a more natural speech style (look at stuff, instead of look stuff)
				action = action.Substring(2).Trim();
			}

			if (string.IsNullOrWhiteSpace(action) || action == "around" || action == "room")
			{
				// Just look at the room, in general
				GameSystem.WriteLine(GameSystem.CurrentMap.CurrentRoom.Describe());
			}
			else if (action == "inventory")
			{
				if (GameSystem.Player.Inventory.Count == 0 && GameSystem.Player.EquippedWeapon == null)
				{
					GameSystem.WriteLine("{0}'s inventory is empty", GameSystem.Player.DisplayName);
				}
				else
				{
					GameSystem.WriteLine("{0}'s inventory:", GameSystem.Player.DisplayName);

					foreach(ILookable item in GameSystem.Player.Inventory)
						GameSystem.WriteLine(item.DisplayName);

					GameSystem.WriteLine("{0} [Equipped]", GameSystem.Player.EquippedWeapon.DisplayName);
				}
			}
			else
			{
				ILookable found = GameSystem.GetLookableWithName(action);

				// Exit if input is invalid (nothing found)
				if (found == null) return false;

				GameSystem.WriteLine(found.Description);
			}

			return false;
		}

		public override void Help()
		{
			GameSystem.WriteLine("Look:");
			GameSystem.WriteLine("\tlook *target*");
			GameSystem.WriteLine("\tlook at *target*");
			GameSystem.WriteLine("\t\ttarget: Who or What to look at");
		}
	}
}
