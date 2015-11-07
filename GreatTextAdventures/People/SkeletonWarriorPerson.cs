using GreatTextAdventures.Items.Weapons;
using System;
using System.Collections.Generic;

namespace GreatTextAdventures.People
{
	class SkeletonWarriorPerson : Person
	{
		const int ExperiencePerLevel = 4;
		const int ExperienceDeltaMultiplier = 15;

		const int HealthMinimum = 20;
		const int HealthPerLevel = 6;

		const int StrengthBase = 5;
		const int StrengthPerLevel = 2;

		const int PDFBase = 5;
		const int PDFPerLevel = 2;

		bool Detected = false;

		public override IEnumerable<string> CodeNames
		{
			get
			{
				yield return "skeleton warrior";
				yield return "skeleton";
				yield return "warrior";
			}
		}

		public override string DisplayName => "&C12Skeleton Warrior&CEE";

		public override int MaxHealth => HealthMinimum + HealthPerLevel * Level;
		public override int MaxMana => 0;

		public SkeletonWarriorPerson(int level) : base(level)
		{
			EquippedWeapon = new SkeletonSwordWeapon(level);

			Strength = StrengthBase + StrengthPerLevel * Level;
			PhysicalDefense = PDFBase + PDFPerLevel * Level;

			Dying += () => GameSystem.WriteLine($"{EquippedWeapon.DisplayName}'s connection with the skeleton has been broken");
			Died += GiveExperience;
		}

		public override void Update()
		{
			base.Update();

			if (Dead) return;

			if (!Detected)
			{
				Detected = true;
				GameSystem.WriteLine($"{DisplayName} has detected {GameSystem.Player.DisplayName}!");
				return;
			}

			Attack(GameSystem.Player);
		}

		void GiveExperience()
		{
			int delta = Level - GameSystem.Player.Level;
			// Math.Max is used to ensure the 'delta bonus' is always positive
			int exp = ExperiencePerLevel * Level + Math.Max(0, delta * ExperienceDeltaMultiplier);

			GameSystem.WriteLine($"{DisplayName} dropped {exp} experience");
			GameSystem.Player.Experience += exp;
		}

		public override void Talk()
		{
			switch (GameSystem.RNG.Next(0, 100))
			{
				case 0:
					GameSystem.WriteLine($"{DisplayName}: NYE HEH HEH!");
					break;
				case 1:
					GameSystem.WriteLine($"{DisplayName}: What's a skeleton's favorite instrument?&W10");
					GameSystem.WriteLine($"{DisplayName}: The trom&W05BONE&W20");
					break;
				case 2:
					GameSystem.WriteLine($"{DisplayName}: Stop wasting my time, I have a TON of work to do&W05.&W05.&W05.&W05");
					GameSystem.WriteLine($"{DisplayName}: A skele&W05-TON&W20");
					break;
				default:
					GameSystem.WriteLine($"{DisplayName} doesn't seem much for conversation");
					break;
			}
		}

	}
}
