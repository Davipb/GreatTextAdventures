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
				yield return "take";
			}
		}

		public override bool Do(string action)
		{
			if (string.IsNullOrWhiteSpace(action))
			{
				Console.WriteLine("Equip what?");
				return false;
			}

			ILookable found = GameSystem.GetMemberWithName(action);

			// If no items were found, return
			if (found == null) return false;

			Items.Weapon weapon = found as Items.Weapon;

			if (weapon == null)
			{
				Console.WriteLine("You can't equip {0}", found.DisplayName);
				ListItemPossibilites(found);
				return false;
			}

			// Drop the currently equipped weapon
			if (GameSystem.Player.EquippedWeapon != null)
			{
				// Warn the player
				Console.WriteLine("You dropped {0}", GameSystem.Player.EquippedWeapon.DisplayName);

				// Add the equipped weapon to the room
				GameSystem.CurrentMap.CurrentRoom.Members.Add(GameSystem.Player.EquippedWeapon);
				// Remove weapon from player's equipped slot
				GameSystem.Player.EquippedWeapon = null;
			}

			// Equip the weapon
			GameSystem.Player.EquippedWeapon = weapon;
			// Remove the weapon from the room
			GameSystem.CurrentMap.CurrentRoom.Members.Remove(found);

			// Warn the player
			Console.WriteLine("You equipped {0}", GameSystem.Player.EquippedWeapon.DisplayName);

			return true;
		}

		public override void Help()
		{
			Console.WriteLine("Equip:");
			Console.WriteLine("\tequip *item*");
			Console.WriteLine("\t\titem: Item to equip");
		}
	}
}
