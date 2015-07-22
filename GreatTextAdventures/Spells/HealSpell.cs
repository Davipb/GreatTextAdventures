using GreatTextAdventures.People;
using System;
using System.Collections.Generic;

namespace GreatTextAdventures.Spells
{
	/// <summary>
	/// Spell that increases the target's health
	/// </summary>
	public class HealSpell : GameSpell
	{
		const int HealPerLevel = 5;
		const int CostPerLevel = 10;
		const float IntBonus = 0.1f;

		public override string DisplayName => $"Heal {level}";
		public override IEnumerable<string> CodeNames
		{
			get 
			{ 
				yield return "heal";
				yield return $"heal {level}";
			}
		}
		public override string Description => $"Heals {HealPerLevel * level} (+{IntBonus * 100f}%/INT) health";
		public override int Cost => CostPerLevel * level;

		public HealSpell(int level) : base(level) { }

		public override bool Cast(Person caster, Person target)
		{
			if (!base.Cast(caster, target)) return false;

			int targetHeal = HealPerLevel * level;
			targetHeal += (int)Math.Ceiling(IntBonus * caster.Intelligence * targetHeal);

			target.Health += targetHeal;
			GameSystem.WriteLine($"{caster.DisplayName} healed {target.DisplayName} for {targetHeal} health");

			return true;
		}
	}
}
