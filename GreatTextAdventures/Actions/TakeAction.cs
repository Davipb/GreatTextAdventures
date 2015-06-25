using System;
using System.Collections.Generic;

namespace GreatTextAdventures.Actions
{
	public class TakeAction : GameAction
	{
		public override IEnumerable<string> Aliases
		{
			get
			{
				yield return "take";
				yield return "pick up";
				yield return "pick";
			}
		}

		public override bool Do(string action)
		{
			if (string.IsNullOrWhiteSpace(action))
			{
				GameSystem.WriteLine("Take what?");
				return false;
			}

			ILookable found = GameSystem.GetLookableWithName(action);

			// If no items were found, return
			if (found == null) return false;

			if (!found.CanTake)
			{
				GameSystem.WriteLine("You can't take {0}", found.DisplayName);
				ListItemPossibilities(found);
				return false;
			}

			if(GameSystem.Player.Inventory.Contains(found) || GameSystem.Player.EquippedWeapon == found)
			{
				GameSystem.WriteLine("{0} is already in your inventory", found.DisplayName);
				return false;
			}

			if(!GameSystem.CurrentMap.CurrentRoom.Members.Contains(found))
			{
				GameSystem.WriteLine("You can only take items that are in the current room");
				return false;
			}

			GameSystem.Player.Inventory.Add(found);
			GameSystem.CurrentMap.CurrentRoom.Members.Remove(found);
			GameSystem.WriteLine("You took {0}", found.DisplayName);

			return true;
		}

		public override void Help()
		{
			GameSystem.WriteLine("Take:");
			GameSystem.WriteLine("\ttake *item*");
			GameSystem.WriteLine("\t\titem: Item to take");
		}
	}
}
