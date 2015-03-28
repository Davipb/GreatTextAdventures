using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

			Initialize();
		}

		public override void Update()
		{
			base.Update();

			if (Health <= 0)
			{
				int delta = this.Level - GameSystem.Player.Level;
				// Math.Max is used to ensure the 'delta bonus' is always positive
				int exp = ExperiencePerLevel * Level + Math.Max(0, delta * ExperienceDeltaMultiplier);

				Console.WriteLine("{0} dropped {1} experience", DisplayName, exp);
				GameSystem.Player.Experience += exp;				

				return;
			}

			Console.WriteLine("{0} attacks you for {1} damage", DisplayName, EquippedWeapon.Attack);
			GameSystem.Player.Health -= EquippedWeapon.Attack;
		}

		public override void Talk()
		{
			Console.WriteLine("\"Ha Ha Ha!\"");
		}
	}
}
