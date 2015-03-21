using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatTextAdventures.People
{
	public class EnemyPerson : Person
	{
		const int ExperiencePerLevel = 3;
		const int ExperienceDeltaMultiplier = 10;

		public override string DisplayName
		{
			get { return "Debug Enemy"; }
		}
		public override IEnumerable<string> CodeNames
		{
			get { yield return "enemy"; }
		}

		public EnemyPerson()
		{
			Health = 20;
			EquippedWeapon = Items.Weapon.Random(this.Level);
		}

		public override void Update()
		{
			base.Update();

			if (Health <= 0)
			{
				int delta = this.Level - GameSystem.Player.Level;

				// Math.Max is used to ensure the 'delta bonus' is always positive
				GameSystem.Player.Experience += ExperiencePerLevel * Level + Math.Max(0, delta * ExperienceDeltaMultiplier);

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
