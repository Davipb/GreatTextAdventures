using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatTextAdventures
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

		public PlayerPerson()
		{
			Health = 100;
			EquippedWeapon = Items.Weapon.Random();
		}

		public override void Talk()
		{
			Console.WriteLine("This is why you don't have friends.");
		}
	}
}
