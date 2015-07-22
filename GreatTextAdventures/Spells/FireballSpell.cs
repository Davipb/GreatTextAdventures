using System;
using System.Collections.Generic;

namespace GreatTextAdventures.Spells
{
	/// <summary>
	/// Spell that damages the target
	/// </summary>
	public class FireballSpell : GameSpell
	{
		const int DamagePerLevel = 5;
		const int CostPerLevel = 10;
		const float IntBonus = 0.05f;

		public override string DisplayName => $"Fireball {level}";
		public override IEnumerable<string> CodeNames
		{
			get
			{
				yield return "fire";
				yield return "fireball";
				yield return "fire ball";
				yield return $"fireball {level}";
			}
		}
		public override string Description =>
					"Hurls a fireball at the enemy, dealing {DamagePerLevel * level} (+{IntBonus * 100f}%/INT) damage";

		public override int Cost => CostPerLevel * level;

		public FireballSpell(int level) : base(level) { }

		public override bool Cast(People.Person caster, People.Person target)
		{
			if (!base.Cast(caster, target)) return false;

			int targetDamage = DamagePerLevel * level;
			targetDamage += (int)Math.Ceiling(IntBonus * caster.Intelligence * targetDamage);

			GameSystem.WriteLine("{0} threw a fireball at {1}", caster.DisplayName, target.DisplayName);

			target.ReceiveDamage(targetDamage, DamageType.Magical, caster);

			return true;
		}
	}
}
