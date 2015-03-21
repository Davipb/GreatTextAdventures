﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatTextAdventures.Actions
{
	public class AttackAction : Action
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
				return false;
			}

			person.Health -= GameSystem.Player.EquippedWeapon.Attack;
			Console.WriteLine("Attacked {0} for {1} damage.", person.DisplayName, GameSystem.Player.EquippedWeapon.Attack);

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
