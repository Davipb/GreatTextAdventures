using GreatTextAdventures.People;
using System;
using System.Collections.Generic;

namespace GreatTextAdventures.Spells
{
	public class HealSpell : GameSpell
	{
		const int HealPerLevel = 5;
		const int CostPerLevel = 10;
		const float IntBonus = 0.1f;

		public override string DisplayName 
		{ 
			get { return string.Format("Heal {0}", level); } 
		}
		public override IEnumerable<string> CodeNames
		{
			get 
			{ 
				yield return "heal";
				yield return string.Format("heal {0}", level);
			}
		}
		public override string Description
		{
			get 
			{ 
				return string.Format("Heals {0} (+{1}%/INT) health", 
					HealPerLevel * level,
					IntBonus * 100f); 
			}
		}
		public override int Cost
		{
			get { return CostPerLevel * level; }
		}

		public HealSpell(int level) : base(level) { }

		public override bool Cast(Person caster, Person target)
		{
			if (!base.Cast(caster, target)) return false;

			int targetHeal = HealPerLevel * level;
			targetHeal += (int)Math.Ceiling(IntBonus * caster.Intelligence * targetHeal);

			target.Health += targetHeal;
			Console.WriteLine("{0} healed {1} for {2} health", caster.DisplayName, target.DisplayName, targetHeal);

			return true;
		}
	}
}
