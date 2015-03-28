using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GreatTextAdventures.People;

namespace GreatTextAdventures.Spells
{
	public class HealSpell : GameSpell
	{
		const int HealPerLevel = 5;
		const int CostPerLevel = 10;

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
			get { return string.Format("Heals {0} health", HealPerLevel * level); }
		}
		public override int Cost
		{
			get { return CostPerLevel * level; }
		}

		public HealSpell(int level) : base(level) { }

		public override bool Cast(Person caster, Person target)
		{
			if (!base.Cast(caster, target)) return false;

			target.Health += HealPerLevel * level;
			Console.WriteLine("{0} healed {1} for {2} health", caster.DisplayName, target.DisplayName, HealPerLevel * level);

			return true;
		}
	}
}
