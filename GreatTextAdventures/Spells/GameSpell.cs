using GreatTextAdventures.People;
using System;
using System.Collections.Generic;

namespace GreatTextAdventures.Spells
{
	public abstract class GameSpell
	{
		protected int level;

		public abstract string DisplayName { get; }
		public abstract IEnumerable<string> CodeNames { get; }
		public abstract string Description { get; }
		public abstract int Cost { get; }

		public GameSpell(int level)
		{
			this.level = Math.Max(1, level);
		}

		public virtual bool Cast(Person caster, Person target)
		{
			if (caster.Mana < Cost)
			{
				GameSystem.WriteLine("Not enough mana!");
				return false;
			}

			caster.Mana -= Cost;

			return true;
		}

		public static IEnumerable<GameSpell> AllSpells(int level)
		{
			yield return new HealSpell(level);
			yield return new FireballSpell(level);
			yield return new ManaHealSpell(level);
			yield return new BloodlustSpell(level);
		}
	}
}
