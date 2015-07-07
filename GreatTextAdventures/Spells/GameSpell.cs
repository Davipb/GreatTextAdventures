using GreatTextAdventures.People;
using System;
using System.Collections.Generic;

namespace GreatTextAdventures.Spells
{
	/// <summary>
	/// Base class for spells that can be cast with Mana by a Person to another Person
	/// </summary>
	public abstract class GameSpell
	{
		// Spell levels allow them to scale with Player levels
		protected int level;

		public abstract string DisplayName { get; }
		public abstract IEnumerable<string> CodeNames { get; }
		public abstract string Description { get; }
		public abstract int Cost { get; }

		public GameSpell(int level)
		{
			this.level = Math.Max(1, level);
		}

		/// <summary>
		/// Make 'caster' cast this spell on 'target'
		/// </summary>
		/// <param name="caster">Person casting the spell</param>
		/// <param name="target">Person being targeted by the spell</param>
		/// <returns>True if the game can be updated after the cast, False otherwise</returns>
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
