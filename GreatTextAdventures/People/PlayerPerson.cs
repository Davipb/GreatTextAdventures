using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatTextAdventures.People
{
	public class PlayerPerson : Person
	{
		public override string DisplayName
		{
			get { return "You"; }
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
			get { return 100 + (Level - 1) * 10; }
		}

		public PlayerPerson() : base()
		{
			Health = MaxHealth;
			EquippedWeapon = Items.Weapon.Random(Level);
		}

		public override void Talk()
		{
			Console.WriteLine("This is why you don't have friends.");
		}
	}
}
