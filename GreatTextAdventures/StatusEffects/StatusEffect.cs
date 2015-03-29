using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GreatTextAdventures.People;

namespace GreatTextAdventures.StatusEffects
{
	public abstract class StatusEffect
	{
		public abstract string DisplayName { get; }
		public abstract string Description { get; }

		public Person Owner { get; set; }

		public StatusEffect(Person owner)
		{
			this.Owner = owner;
		}

		public abstract void Update();
	}
}
