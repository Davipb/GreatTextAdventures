using GreatTextAdventures.People;
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

		public override string DisplayName => $"Poison Cloud {level}";
		public override IEnumerable<string> CodeNames
		{
			get
			{
				yield return "poison cloud";
				yield return "poison";
				yield return $"poison cloud {level}";
			}
		}
		public override string Description => "Poisons the target, dealing a percentage of their health in damage each turn";
		public override int Cost => CostMinimum + CostPerLevel * (level - 1);
		public int Duration => DurationMinimum + DurationPerLevel * (level - 1);

		public PoisonSpell(int level) : base(level) { }

		public override bool Cast(Person caster, Person target)
		{
			if (!base.Cast(caster, target)) return false;

			if(target.CurrentStatus.Any(x => x is PoisonEffect))
			{
				GameSystem.WriteLine($"{target.DisplayName} is already poisoned!");
				return false;
			}

			GameSystem.WriteLine($"{caster.DisplayName} used {DisplayName} on {target}!");
			target.CurrentStatus.Add(new PoisonEffect(target, Duration));
			GameSystem.WriteLine($"{target.DisplayName} was poisoned!");

			return true;
		}
	}
}
