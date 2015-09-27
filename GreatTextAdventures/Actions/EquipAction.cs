using GreatTextAdventures.Items.Weapons;
using System;
using System.Collections.Generic;

namespace GreatTextAdventures.Actions
{
	public class EquipAction : GameAction
	{
		public override IEnumerable<string> Aliases
		{
			get
			{
				yield return "equip";
			}
		}

		public override bool Do(string action)
		{
			if (string.IsNullOrWhiteSpace(action))
			{
				GameSystem.WriteLine("Equip what?");
				return false;
			}

			ILookable found = GameSystem.GetLookableWithName(action);

			// If no items were found, return
			if (found == null) return false;

			var weapon = found as Weapon;

			if (weapon == null)
			{
				GameSystem.WriteLine($"You can't equip {found.DisplayName}");
				ListItemPossibilities(found);
				return false;
			}

			// Put equipped weapon back in the Inventory
			if (GameSystem.Player.EquippedWeapon != null)
			{
				GameSystem.WriteLine($"You put {GameSystem.Player.EquippedWeapon.DisplayName} back in your Inventory");
				GameSystem.Player.Inventory.Add(GameSystem.Player.EquippedWeapon);

				GameSystem.Player.EquippedWeapon = null;
			}

			// Equip the weapon
			GameSystem.Player.EquippedWeapon = weapon;
			// Remove the weapon from the room
			GameSystem.CurrentMap.CurrentRoom.Members.Remove(found);

			// Warn the player
			GameSystem.WriteLine($"You equipped {GameSystem.Player.EquippedWeapon.DisplayName}");

			return true;
		}

		public override void Help()
		{
			GameSystem.WriteLine("Equip:");
			GameSystem.WriteLine("\tequip *item*");
			GameSystem.WriteLine("\t\titem: Item to equip");
		}
	}
}
