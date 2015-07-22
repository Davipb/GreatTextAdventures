using GreatTextAdventures.People;
using System;

namespace GreatTextAdventures.StatusEffects
{
	/// <summary>
	/// Effect that causes a percentage of the Person's health in damage every turn
	/// </summary>
	public class PoisonEffect : StatusEffect
	{
		const double HealthToDrain = 0.05;

		public override string DisplayName => "Poison";
		public override string Description => $"Damages {HealthToDrain * 100.0}% of max health per turn";

		public PoisonEffect(Person owner, int duration) : base(owner, duration) { }

		public override void Update()
		{
			// Must damage at least 1 Health
			int damage = Math.Max(1, (int)Math.Floor(Owner.MaxHealth * HealthToDrain));

			GameSystem.WriteLine($"{Owner.DisplayName} is poisoned");

			Owner.ReceiveDamage(damage, DamageType.Magical, this);

			base.Update();
		}
	}
}
