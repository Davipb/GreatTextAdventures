using GreatTextAdventures.People;
using System;
using System.Collections.Generic;

namespace GreatTextAdventures.Items
{
	public class ManaFountain : ILookable, IUsable
	{
		public string DisplayName { get { return "Mana Fountain"; } }
		public string Description 
		{ 
			get
			{
				return "A magical fountain that can restore your health";
			}
		}
		public IEnumerable<string> CodeNames
		{
			get
			{
				yield return "mana fountain";
				yield return "fountain";
				yield return "mana";
			}
		}

		public bool CanTake { get { return false; } }

		public void Use(Person user)
		{
			GameSystem.WriteLine("{0}'s mana was completely restored", user.DisplayName);
			user.Mana = user.MaxMana;
		}

		public void Update() { }

	}
}
