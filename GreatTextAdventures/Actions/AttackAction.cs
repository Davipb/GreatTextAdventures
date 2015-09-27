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
				GameSystem.WriteLine("Attack what?");
				return false;
			}

			ILookable found = GameSystem.GetLookableWithName(action);

			// Exit if input is invalid (nothing found)
			if (found == null) return false;

			var person = found as Person;

			if (person == null)
			{
				GameSystem.WriteLine($"You can't attack {found.DisplayName}");
				ListItemPossibilities(found);
				return false;
			}
			GameSystem.Player.Attack(person);
			return true;
		}

		public override void Help()
		{
			GameSystem.WriteLine("Attack:");
			GameSystem.WriteLine("\tattack *target*");
			GameSystem.WriteLine("\t\ttarget: Who to attack");
		}
	}
}
