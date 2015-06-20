using GreatTextAdventures.People;
using System;
using System.Collections.Generic;

namespace GreatTextAdventures.Actions
{
	public class AttackAction : GameAction
	{
		public override IEnumerable<string> Aliases
		{
			get 
			{
				yield return "attack";
				yield return "kill";
				yield return "hit";
			}
		}

		public override bool Do(string action)
		{
			if (string.IsNullOrWhiteSpace(action))
			{				
				Console.WriteLine("Attack what?");
				return false;
			}

			ILookable found = GameSystem.GetMemberWithName(action);

			// Exit if input is invalid (nothing found)
			if (found == null) return false;

			Person person = found as Person;

			if (person == null)
			{
				Console.WriteLine("You can't attack {0}", found.DisplayName);
				ListItemPossibilities(found);
				return false;
			}

			Console.WriteLine("{0} attacked {1}", GameSystem.Player.DisplayName, person.DisplayName);
			person.ReceiveDamage(
				GameSystem.Player.EquippedWeapon == null ? 1 : GameSystem.Player.EquippedWeapon.Damage(GameSystem.Player), 
				DamageType.Physical,
				GameSystem.Player);

			return true;
		}

		public override void Help()
		{
			Console.WriteLine("Attack:");
			Console.WriteLine("\tattack *target*");
			Console.WriteLine("\t\ttarget: Who to attack");
		}
	}
}
