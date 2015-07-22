using GreatTextAdventures.People;
using System;
using System.Collections.Generic;

namespace GreatTextAdventures.Spells
{
	/// <summary>
	/// Spell that damages both the caster and the target
	/// </summary>
	public class BloodlustSpell : GameSpell
	{
		const int CostPerLevel = 5;
		const int HealthPerLevel = 7;
		const float DamageBonusBase = 1.5f;
		const float DamageBonusPerLevel = 0.2f;

		public override string DisplayName => $"Bloodlust {level}";
		public override IEnumerable<string> CodeNames
		{
			get
			{
				yield return "bloodlust";
				yield return $"bloodlust {level}";
			}
		}
		public override string Description =>
					$"Damages yourself for {HealthPerLevel * level} damage and the target for {(DamageBonusBase + DamageBonusPerLevel * level) * 100f}% normal damage";

		public override int Cost => CostPerLevel * level;

		public BloodlustSpell(int level) : base(level) { }

		public override bool Cast(Person caster, Person target)
		{
			if (caster.Health < HealthPerLevel * level)
			{
				GameSystem.WriteLine("Not enough health!");
				return false;
			}

			if (!base.Cast(caster, target)) return false;

			int casterDamage = HealthPerLevel * level;

			GameSystem.WriteLine($"{caster.DisplayName} used {DisplayName} at {target.DisplayName}!");

			int targetDamage = caster.EquippedWeapon.Damage(caster);
			targetDamage = (int)Math.Ceiling(targetDamage * (DamageBonusBase + DamageBonusPerLevel * level));

			caster.ReceiveDamage(casterDamage, DamageType.Special, caster);
			target.ReceiveDamage(targetDamage, DamageType.Physical, target);

			return true;
		}
	}
}
