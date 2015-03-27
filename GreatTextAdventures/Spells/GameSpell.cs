using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GreatTextAdventures.People;

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
			this.level = level;
		}

		public abstract bool Cast(Person caster, Person target);
	}
}
