using GreatTextAdventures.People;
using System;
using System.Collections.Generic;

namespace GreatTextAdventures.Spells
{
	/// <summary>
	/// Spell that exchanges health for mana on the caster
	/// </summary>
	public class ManaHealSpell : GameSpell
	{
		const int HealthPerLevel = 10;
		const int ManaPerLevel = 20;

		public override string DisplayName => $"Refresh {level}";
		public override IEnumerable<string> CodeNames 
		{ 
			get 
			{ 
				yield return "refresh";
				yield return $"refresh {level}";
			} 
		}
		public override string Description => $"Damage {HealthCost} health and restore {ManaHeal} mana";
		public override int Cost => 0;
		public int HealthCost => HealthPerLevel * level;
		public int ManaHeal => ManaPerLevel * level;

		public ManaHealSpell(int level) : base(level) { }

		public override bool Cast(Person caster, Person target)
		{
			if (caster != target)
			{
				GameSystem.WriteLine("Can only cast at yourself");
				return false;
			}

			if (target.Health < HealthPerLevel * level)
			{
				GameSystem.WriteLine("Not enough health!");
				return false;
			}

			GameSystem.WriteLine($"{caster.DisplayName} used {DisplayName}!");

			target.ReceiveDamage(HealthCost, DamageType.Special, this);
			target.Mana += ManaHeal;

			GameSystem.WriteLine($"{target.DisplayName} recovered {ManaHeal} mana");

			return true;
		}
	}
}
