using GreatTextAdventures.Items.Weapons;
using System;
using System.Collections.Generic;

namespace GreatTextAdventures.People
{
	public class ThugPerson : Person
	{
		const int ExperiencePerLevel = 3;
		const int ExperienceDeltaMultiplier = 10;

		const int HealthMinimum = 15;
		const int HealthPerLevel = 5;

		const int ManaMinimum = 10;
		const int ManaPerLevel = 5;

		const int StrMinPerLevel = 1;
		const int StrMaxPerLevel = 2;

		bool detected = false;

		public override string DisplayName => $"Thug (Lv {Level})";

		public override IEnumerable<string> CodeNames
		{
			get { yield return "thug"; }
		}

		public override int MaxHealth => HealthMinimum + (Level - 1) * HealthPerLevel;

		public override int MaxMana => ManaMinimum + (Level - 1) * ManaPerLevel;

		public ThugPerson(int level) : base(level)
		{			
			EquippedWeapon = RandomWeapon.Generate(Level);

			Strength = GameSystem.RNG.Next(StrMinPerLevel * level, StrMinPerLevel * level);
			Intelligence = 0;
		}

		public override void Update()
		{
			base.Update();

			if (Health <= 0)
			{
				int delta = Level - GameSystem.Player.Level;
				// Math.Max is used to ensure the 'delta bonus' is always positive
				int exp = ExperiencePerLevel * Level + Math.Max(0, delta * ExperienceDeltaMultiplier);

				GameSystem.WriteLine($"{DisplayName} dropped {exp} experience");
				GameSystem.Player.Experience += exp;				

				return;
			}

			if (!detected)
			{
				detected = true;
				GameSystem.WriteLine($"{DisplayName} has detected {GameSystem.Player.DisplayName}!");
				return;
			}

			Attack(GameSystem.Player);

		}

		public override void Talk()
		{
			GameSystem.WriteLine("\"Ha Ha Ha!\"");
		}
	}
}
