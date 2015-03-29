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
		public int Duration { get; set; }
		protected int currentDuration = 0;

		public StatusEffect(Person owner, int duration)
		{
			this.Owner = owner;
			this.Duration = duration;
		}

		public virtual void Update()
		{
			currentDuration++;
			
			if (currentDuration >= Duration)
			{
				Owner.CurrentStatus.Remove(this);
				Console.WriteLine("{0}'s {1} wore off", Owner.DisplayName, DisplayName);
			}
		}
	}
}
