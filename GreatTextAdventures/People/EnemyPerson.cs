using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatTextAdventures.People
{
	public class EnemyPerson : Person
	{
		public EnemyPerson()
		{
			DisplayName = "Enemy";
			CodeNames = new[] { "enemy" };
			Description = "DEBUG ENEMY";

			Health = 20;
			EquippedWeapon = Items.Weapon.Random();
		}

		public override void Update()
		{
			base.Update();

			Console.WriteLine("{0} attacks you for {1} damage", DisplayName, EquippedWeapon.Attack);
			GameSystem.Player.Health -= EquippedWeapon.Attack;
		}

		public override void Talk()
		{
			Console.WriteLine("\"Ha Ha Ha!\"");
		}
	}
}
