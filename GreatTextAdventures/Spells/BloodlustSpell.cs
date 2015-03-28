using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GreatTextAdventures.People;

namespace GreatTextAdventures.Spells
{
	public class BloodlustSpell : GameSpell
	{
		const int CostPerLevel = 5;
		const int HealthPerLevel = 7;
		const int DamagePerLevel = 15;

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
					"Damages yourself for {0} damage and the target for {1} damage", 
					HealthPerLevel * level, 
					DamagePerLevel * level);
			}
		}
		public override int Cost { get { return CostPerLevel * level; } }

		public BloodlustSpell(int level) : base(level) { }

		public override bool Cast(Person caster, Person target)
		{
			if (caster.Health < HealthPerLevel * level)
			{
				Console.WriteLine("Not enough health!");
				return false;
			}

			if (!base.Cast(caster, target)) return false;

			caster.Health -= HealthPerLevel * level;
			Console.WriteLine("{0} was damaged for {1} health", caster.DisplayName, HealthPerLevel * level);

			target.Health -= DamagePerLevel * level;
			Console.WriteLine("{0} was damaged for {1} health", target.DisplayName, DamagePerLevel * level);

			return true;
		}
	}
}
