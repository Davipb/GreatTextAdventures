using GreatTextAdventures.People;
using System;

namespace GreatTextAdventures.StatusEffects
{
	public class PoisonEffect : StatusEffect
	{
		const double HealthToDrain = 0.05;

		public override string DisplayName { get { return "Poison"; } }
		public override string Description
		{
			get { return string.Format("Damages {0}% of max health per turn", HealthToDrain * 100.0); }
		}

		public PoisonEffect(Person owner, int duration) : base(owner, duration) { }

		public override void Update()
		{
			// Must damage at least 1 Health
			int damage = Math.Max(1, (int)Math.Floor(Owner.MaxHealth * HealthToDrain));
			Console.WriteLine("{0} is poisoned, lost {1} health", Owner.DisplayName, damage);

			Owner.Health -= damage;

			base.Update();
		}
	}
}
