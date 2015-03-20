using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatTextAdventures.People
{
	public class EnemyPerson : Person
	{
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
			EquippedWeapon = Items.Weapon.Random();
		}

		public override void Update()
		{
			base.Update();

			if (Health <= 0) return;

			Console.WriteLine("{0} attacks you for {1} damage", DisplayName, EquippedWeapon.Attack);
			GameSystem.Player.Health -= EquippedWeapon.Attack;
		}

		public override void Talk()
		{
			Console.WriteLine("\"Ha Ha Ha!\"");
		}
	}
}
