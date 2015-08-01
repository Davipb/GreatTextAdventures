using GreatTextAdventures.People;
using System;
using System.Collections.Generic;

namespace GreatTextAdventures.Items
{
	public class ManaFountain : ILookable, IUsable
	{
		public string DisplayName => "Mana Fountain";
		public string Description => "A magical fountain that can restore your health";
		public IEnumerable<string> CodeNames
		{
			get
			{
				yield return "mana fountain";
				yield return "fountain";
				yield return "mana";
			}
		}

		public bool CanTake => false;

		public void Use(Person user)
		{
			GameSystem.WriteLine($"{user.DisplayName}'s mana was completely restored");
			user.Mana = user.MaxMana;
		}
	}
}
