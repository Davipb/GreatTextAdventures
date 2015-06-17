using GreatTextAdventures.People;
using System;
using System.Collections.Generic;

namespace GreatTextAdventures.Spells
{
	public class BloodlustSpell : GameSpell
	{
		const int CostPerLevel = 5;
		const int HealthPerLevel = 7;
		const int DamagePerLevel = 15;
		const float IntHealthBonus = 0.01f;
		const float IntDamageBonus = 0.1f;

		public override string DisplayName { get { return string.Format("Bloodlust {0}", level); } }
		public override IEnumerable<string> CodeNames
		{
			get
			{
				yield return "bloodlust";
				yield return string.Format("bloodlust {0}", level);
			}
		}
		public override string Description
		{
			get
			{
				return string.Format(
					"Damages yourself for {0} (-{2}%/INT) damage and the target for {1} (+{3}%/INT) damage", 
					HealthPerLevel * level, 
					DamagePerLevel * level,
					IntHealthBonus * 100f,
					IntDamageBonus * 100f);
			}
		}
		public override int Cost { get { return CostPerLevel * level; } }

		public BloodlustSpell(int level) : base(level) { }

		public override bool Cast(Person caster, Person target)
		{
			if (caster.Health < HealthPerLevel * level)
			{
				Console.WriteLine("Not enough health!");
				return false;
			}

			if (!base.Cast(caster, target)) return false;

			int casterDamage = HealthPerLevel * level;
			casterDamage += (int)Math.Ceiling(IntHealthBonus * caster.Intelligence * casterDamage);

			int targetDamage = DamagePerLevel * level;
			targetDamage = (int)Math.Ceiling(IntDamageBonus * caster.Intelligence * targetDamage);
			targetDamage -= target.Defense;
			targetDamage = Math.Max(0, targetDamage);

			Console.WriteLine("{0} used {1} at {2}!", caster.DisplayName, this.DisplayName, target.DisplayName);
						
			Console.WriteLine("{0} was damaged for {1} health", caster.DisplayName, casterDamage);
			caster.Health -= casterDamage;
						
			Console.WriteLine("{0} was damaged for {1} health", target.DisplayName, targetDamage);
			target.Health -= targetDamage;

			return true;
		}
	}
}
