﻿using System;
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
				GameSystem.WriteLine("Equip what?");
				return false;
			}

			ILookable found = GameSystem.GetMemberWithName(action);

			// If no items were found, return
			if (found == null) return false;

			Items.Weapon weapon = found as Items.Weapon;

			if (weapon == null)
			{
				GameSystem.WriteLine("You can't equip {0}", found.DisplayName);
				ListItemPossibilities(found);
				return false;
			}

			// Drop the currently equipped weapon
			if (GameSystem.Player.EquippedWeapon != null)
			{
				// Warn the player
				GameSystem.WriteLine("You dropped {0}", GameSystem.Player.EquippedWeapon.DisplayName);

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
			GameSystem.WriteLine("You equipped {0}", GameSystem.Player.EquippedWeapon.DisplayName);

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
