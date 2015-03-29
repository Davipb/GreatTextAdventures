using System;
using System.Collections.Generic;

namespace GreatTextAdventures.Spells
{
	public class FireballSpell : GameSpell
	{
		const int DamagePerLevel = 15;
		const int CostPerLevel = 10;

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
					"Hurls a fireball at the enemy, dealing {0} damage", 
					DamagePerLevel * level); 
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

			Console.WriteLine("{0} threw a fireball at {1} for {2} damage", caster.DisplayName, target.DisplayName, DamagePerLevel * level);
			target.Health -= DamagePerLevel * level;

			return true;
		}
	}
}
