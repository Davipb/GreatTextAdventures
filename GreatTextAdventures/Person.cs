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

		private int health;
		public int Health
		{
			get { return health; }
			set { health = Math.Max(value, 0); }
		}
		public Weapon EquippedWeapon { get; set; }

		public virtual void Update() 
		{ 
			if (Health <= 0)
			{
				Console.WriteLine("{0} died", DisplayName);

				if (EquippedWeapon != null)
				{
					Console.WriteLine("{0} dropped {1} ({2})", DisplayName, EquippedWeapon.DisplayName, EquippedWeapon.Attack);

					GameSystem.CurrentMap.CurrentRoom.Members.Add(EquippedWeapon);
					EquippedWeapon = null;
				}

				GameSystem.CurrentMap.CurrentRoom.Members.Remove(this);
			}
		}

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
