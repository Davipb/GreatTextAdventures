using GreatTextAdventures.People;

namespace GreatTextAdventures.StatusEffects
{
	/// <summary>
	/// Base class for temporary effects applied on a Person
	/// </summary>
	public abstract class StatusEffect : IUpdatable
	{
		public abstract string DisplayName { get; }
		public abstract string Description { get; }

		public Person Owner { get; set; }
		public int Duration { get; set; }
		protected int currentDuration = 0;

		protected StatusEffect(Person owner, int duration)
		{
			Owner = owner;
			Duration = duration;
		}

		public virtual void Update()
		{
			if (++currentDuration >= Duration)
			{
				Owner.CurrentStatus.Remove(this);
				GameSystem.WriteLine($"{Owner.DisplayName}'s {DisplayName} wore off");
			}
		}
	}
}
