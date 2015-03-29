﻿using GreatTextAdventures.People;
using System;
using System.Collections.Generic;

namespace GreatTextAdventures.Spells
{
	public class ManaHealSpell : GameSpell
	{
		const int HealthPerLevel = 10;
		const int ManaPerLevel = 20;

		public override string DisplayName { get { return string.Format("Refresh {0}", level); } }
		public override IEnumerable<string> CodeNames 
		{ 
			get 
			{ 
				yield return "refresh";
				yield return string.Format("refresh {0}", level);
			} 
		}
		public override string Description { get { return string.Format("Damage {0} health and restore {1} mana", HealthCost, ManaHeal); } }
		public override int Cost { get { return 0; } }
		public int HealthCost { get { return HealthPerLevel * level; } }
		public int ManaHeal { get { return ManaPerLevel * level; } }

		public ManaHealSpell(int level) : base(level) { }

		public override bool Cast(Person caster, Person target)
		{
			if (caster != target)
			{
				Console.WriteLine("Can only cast at yourself");
				return false;
			}

			if (target.Health < HealthPerLevel * level)
			{
				Console.WriteLine("Not enough health!");
				return false;
			}

			target.Health -= HealthCost;
			target.Mana += ManaHeal;

			Console.WriteLine("{0} used {1}!", caster.DisplayName, this.DisplayName);
			Console.WriteLine("{0} lost {1} health", target.DisplayName, HealthCost);
			Console.WriteLine("{0} recovered {1} mana", target.DisplayName, ManaHeal);

			return true;
		}
	}
}
