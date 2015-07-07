﻿using GreatTextAdventures.People;
using GreatTextAdventures.StatusEffects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GreatTextAdventures.Spells
{
	/// <summary>
	/// Spell that applies the Poison status effect to the target
	/// </summary>
	public class PoisonSpell : GameSpell
	{
		const int DurationMinimum = 3;
		const int DurationPerLevel = 1;
		const int CostMinimum = 20;
		const int CostPerLevel = 10;

		public override string DisplayName
		{
			get { return string.Format("Poison Cloud {0}", level); }
		}
		public override IEnumerable<string> CodeNames
		{
			get
			{
				yield return "poison cloud";
				yield return "poison";
				yield return string.Format("poison cloud {0}", level);
			}
		}
		public override string Description
		{
			get { return "Poisons the target, dealing a percentage of their health in damage each turn"; }
		}
		public override int Cost
		{
			get { return CostMinimum + CostPerLevel * (level - 1); }
		}
		public int Duration
		{
			get { return DurationMinimum + DurationPerLevel * (level - 1); }
		}

		public PoisonSpell(int level) : base(level) { }

		public override bool Cast(Person caster, Person target)
		{
			if (!base.Cast(caster, target)) return false;

			if(target.CurrentStatus.Any(x => x is PoisonEffect))
			{
				GameSystem.WriteLine("{0} is already poisoned!", target.DisplayName);
				return false;
			}

			GameSystem.WriteLine("{0} used {1} on {2}!", caster.DisplayName, DisplayName, target);
			target.CurrentStatus.Add(new PoisonEffect(target, Duration));
			GameSystem.WriteLine("{0} was poisoned!", target.DisplayName);

			return true;
		}
	}
}
