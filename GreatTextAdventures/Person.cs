using GreatTextAdventures.Items;
using System;
using System.Collections.Generic;

namespace GreatTextAdventures
{
	public abstract class Person : ILookable
	{
		public string DisplayName { get; set; }
		public IEnumerable<string> CodeNames { get; set; }
		public string Description { get; set; }

		public int Health { get; set; }
		public Weapon EquippedWeapon { get; set; }

		public virtual void Update() { /* ¯\_(ツ)_/¯ */ }

		public Person()
		{
			DisplayName = "ERROR";
			CodeNames = new[] { "error" };
			Description = "SOMETHING BROKE";

			Health = 0;
			EquippedWeapon = null;
		}

		public virtual void Talk()
		{
			Console.WriteLine("{0} doesn't answer", DisplayName);
		}
	}
}
