using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatTextAdventures
{
	public class PlayerPerson : Person
	{
		public PlayerPerson()
		{
			DisplayName = "You";
			CodeNames = new[] { "self", "me" };
			Description = "How can a being describe oneself? Is there such thing as existence?";

			Health = 100;
			EquippedWeapon = Items.Weapon.Random();
		}

		public override void Talk()
		{
			Console.WriteLine("This is why you don't have friends.");
		}
	}
}
