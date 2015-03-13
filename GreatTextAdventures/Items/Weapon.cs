using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatTextAdventures.Items
{
	public class Weapon : Item
	{
		public int Attack { get; set; }

		public Weapon()
		{
			CanTake = true;
		}

		public static Weapon Random()
		{
			Weapon result = new Weapon();
			result.DisplayName = "Sword";
			result.CodeNames = new[] { "sword" };
			result.Attack = 10;
			result.Description = "A generic sword.";
			
			return result;
		}
	}
}
