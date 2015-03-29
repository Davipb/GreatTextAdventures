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

		public override string DisplayName
		{
			get { return "Player"; }
		}
		public override IEnumerable<string> CodeNames
		{
			get
			{
				yield return "self";
				yield return "me";
			}
		}

		public override int MaxHealth
		{
			get { return HealthMinimum + (Level - 1) * HealthPerLevel; }
		}

		public override int MaxMana
		{
			get { return ManaMinimum + (Level - 1) * ManaPerLevel; }
		}

		public PlayerPerson()
		{
			EquippedWeapon = Items.Weapon.Random(Level);

			Initialize();
		}

		public override void Update()
		{
			base.Update();
		}

		public override void Talk()
		{
			Console.WriteLine("This is why you don't have friends.");
		}
	}
}
