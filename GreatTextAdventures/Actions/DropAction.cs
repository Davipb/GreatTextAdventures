using System;
using System.Collections.Generic;

namespace GreatTextAdventures.Actions
{
	public class DropAction : GameAction
	{
		public override IEnumerable<string> Aliases
		{
			get
			{
				yield return "drop";
				yield return "discard";
				yield return "throw away";
			}
		}

		public override bool Do(string action)
		{
			if (string.IsNullOrWhiteSpace(action))
			{
				GameSystem.WriteLine("Drop what?");
				return false;
			}

			ILookable found = GameSystem.GetLookableWithName(action);

			// If no items were found, return
			if (found == null) return false;

			if (GameSystem.Player.Inventory.Contains(found))
			{
				GameSystem.Player.Inventory.Remove(found);
			}
			else if (GameSystem.Player.EquippedWeapon == found)
			{
				GameSystem.Player.EquippedWeapon = null;
			}
			else
			{
				GameSystem.WriteLine("You can only drop items that are in your inventory", found.DisplayName);
				return false;
			}
			
			GameSystem.CurrentMap.CurrentRoom.Members.Add(found);
			GameSystem.WriteLine("You dropped {0}", found.DisplayName);

			return true;
		}

		public override void Help()
		{
			GameSystem.WriteLine("Drop:");
			GameSystem.WriteLine("\tdrop *item*");
			GameSystem.WriteLine("\t\titem: Item to drop");
		}
	}
}
