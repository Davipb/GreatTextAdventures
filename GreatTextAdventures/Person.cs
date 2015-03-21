using GreatTextAdventures.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace GreatTextAdventures
{
	public abstract class Person : ILookable
	{
		public abstract string DisplayName { get; }
		public abstract IEnumerable<string> CodeNames { get; }

		public string Description 
		{ 
			get
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendLine(DisplayName);
				sb.AppendFormat("Level: {0}", Level);
				sb.AppendLine();
				sb.AppendFormat("Experience: {0}/{1}", Experience, NeededExperience);
				sb.AppendLine();
				sb.AppendFormat("Health: {0}", Health);
				sb.AppendLine();
				sb.AppendFormat("Weapon: {0}", EquippedWeapon == null ? "None" : EquippedWeapon.DisplayName);

				return sb.ToString();				
			}
		}

		private int health;
		public int Health
		{
			get { return health; }
			set { health = Math.Max(value, 0); }
		}

		public Weapon EquippedWeapon { get; set; }

		protected int level = 1;
		public int Level
		{
			get { return level; }
			set 
			{ 
				level = Math.Max(1, value);
			}
		}

		protected long experience = 0;
		public long Experience
		{
			get { return experience; }
			set
			{
				experience = value;
				while (experience >= NeededExperience)
				{
					experience -= NeededExperience;
					Level++;					
					Console.WriteLine("{0} grew to level {1}!", DisplayName, Level);
				}
			}
		}
		public long NeededExperience { get { return Level * Level; } }

		public virtual void Update() 
		{ 
			if (Health <= 0)
			{
				Console.WriteLine("{0} died", DisplayName);

				if (EquippedWeapon != null)
				{
					Console.WriteLine("{0} dropped {1}", DisplayName, EquippedWeapon.DisplayName);

					GameSystem.CurrentMap.CurrentRoom.Members.Add(EquippedWeapon);
					EquippedWeapon = null;
				}

				GameSystem.CurrentMap.CurrentRoom.Members.Remove(this);
			}
		}

		public Person()
		{
			Health = 0;
			EquippedWeapon = null;
		}

		public virtual void Talk()
		{
			Console.WriteLine("{0} doesn't answer", DisplayName);
		}
	}
}
