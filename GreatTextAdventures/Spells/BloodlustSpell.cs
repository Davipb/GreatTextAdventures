using GreatTextAdventures.People;
using System;
using System.Collections.Generic;

namespace GreatTextAdventures.Spells
{
	public class BloodlustSpell : GameSpell
	{
		const int CostPerLevel = 5;
		const int HealthPerLevel = 7;
		const float DamageBonusBase = 1.5f;
		const float DamageBonusPerLevel = 0.2f;

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
					"Damages yourself for {0} damage and the target for {1}% normal damage", 
					HealthPerLevel * level, 
					(DamageBonusBase + DamageBonusPerLevel * level) * 100f);
			}
		}
		public override int Cost { get { return CostPerLevel * level; } }

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

			GameSystem.WriteLine("{0} used {1} at {2}!", caster.DisplayName, this.DisplayName, target.DisplayName);

			int targetDamage = caster.EquippedWeapon.Damage(caster);
			targetDamage = (int)Math.Ceiling(targetDamage * (DamageBonusBase + DamageBonusPerLevel * level));

			caster.ReceiveDamage(casterDamage, DamageType.Special, caster);
			target.ReceiveDamage(targetDamage, DamageType.Physical, target);

			return true;
		}
	}
}
