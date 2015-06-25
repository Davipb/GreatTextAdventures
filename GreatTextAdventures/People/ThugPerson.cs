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

		public override string DisplayName
		{
			get { return string.Format("Thug (Lv {0})", Level); }
		}

		public override IEnumerable<string> CodeNames
		{
			get { yield return "thug"; }
		}

		public override int MaxHealth
		{
			get { return HealthMinimum + (Level - 1) * HealthPerLevel; }
		}

		public override int MaxMana
		{
			get { return ManaMinimum + (Level - 1) * ManaPerLevel; }
		}

		public ThugPerson(int level) : base()
		{			
			Level = level;
			EquippedWeapon = Items.Weapon.Random(this.Level);

			Strength = GameSystem.RNG.Next(StrMinPerLevel * level, StrMinPerLevel * level);
			Intelligence = 0;
		}

		public override void Update()
		{
			base.Update();

			if (Health <= 0)
			{
				int delta = this.Level - GameSystem.Player.Level;
				// Math.Max is used to ensure the 'delta bonus' is always positive
				int exp = ExperiencePerLevel * Level + Math.Max(0, delta * ExperienceDeltaMultiplier);

				GameSystem.WriteLine("{0} dropped {1} experience", DisplayName, exp);
				GameSystem.Player.Experience += exp;				

				return;
			}

			if (!detected)
			{
				detected = true;
				GameSystem.WriteLine("{0} has detected {1}!", DisplayName, GameSystem.Player.DisplayName);
				return;
			}

			GameSystem.WriteLine("{0} attacked {1} with {2}", DisplayName, GameSystem.Player.DisplayName, EquippedWeapon.DisplayName);
			GameSystem.Player.ReceiveDamage(EquippedWeapon.Damage(this), DamageType.Physical, this);

		}

		public override void Talk()
		{
			GameSystem.WriteLine("\"Ha Ha Ha!\"");
		}
	}
}
