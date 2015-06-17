using System;
using System.Collections.Generic;

namespace GreatTextAdventures.Spells
{
	public class FireballSpell : GameSpell
	{
		const int DamagePerLevel = 5;
		const int CostPerLevel = 10;
		const float IntBonus = 0.05f;

		public override string DisplayName 
		{ 
			get { return string.Format("Fireball {0}", level); } 
		}
		public override IEnumerable<string> CodeNames
		{
			get 
			{
				yield return "fire";
				yield return "fireball";
				yield return "fire ball";
				yield return string.Format("fireball {0}", level);
			}
		}
		public override string Description 
		{ 
			get 
			{ 
				return string.Format(
					"Hurls a fireball at the enemy, dealing {0} (+{1}%/INT) damage", 
					DamagePerLevel * level,
					IntBonus * 100f); 
			} 
		}
		public override int Cost 
		{ 
			get { return CostPerLevel * level; } 
		}

		public FireballSpell(int level) : base(level) { }

		public override bool Cast(People.Person caster, People.Person target)
		{
			if (!base.Cast(caster, target)) return false;

			int targetDamage = DamagePerLevel * level;
			targetDamage += (int)Math.Ceiling(IntBonus * caster.Intelligence * targetDamage);

			Console.WriteLine("{0} threw a fireball at {1} for {2} damage", caster.DisplayName, target.DisplayName, targetDamage);
			target.Health -= targetDamage;

			return true;
		}
	}
}
