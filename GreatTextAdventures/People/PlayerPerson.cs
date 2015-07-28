using GreatTextAdventures.Items.Weapons;
using System;
using System.Collections.Generic;

namespace GreatTextAdventures.People
{
	public class PlayerPerson : Person
	{
		const int HealthMinimum = 100;
		const int HealthPerLevel = 10;
		const int ManaMinimum = 50;
		const int ManaPerLevel = 5;

		public override string DisplayName => "Player";
		public override IEnumerable<string> CodeNames
		{
			get
			{
				yield return "self";
				yield return "me";
			}
		}
		public override int MaxHealth => HealthMinimum + (Level - 1) * HealthPerLevel;
		public override int MaxMana => ManaMinimum + (Level - 1) * ManaPerLevel;

		public int PendingSkillPoints { get; set; }

		public PlayerPerson() : base()
		{
			EquippedWeapon = RandomWeapon.Random(Level);
			LeveledUp += LevelUpEventHandler;
		}

		public override void Update()
		{
			base.Update();
		}

		public override void Talk()
		{
			GameSystem.WriteLine("This is why you don't have friends.");
		}

		protected void LevelUpEventHandler()
		{
			PendingSkillPoints += 2;
			GameSystem.WriteLine($"+2 Skill Points ({PendingSkillPoints} total)");
		}
	}
}
