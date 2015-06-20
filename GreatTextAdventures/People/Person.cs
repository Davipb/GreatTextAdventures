using GreatTextAdventures.Items;
using GreatTextAdventures.Spells;
using GreatTextAdventures.StatusEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GreatTextAdventures.People
{
	public abstract class Person : ILookable
	{
		#region Properties
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
				sb.AppendFormat("Health (HP): {0}/{1}", Health, MaxHealth);
				sb.AppendLine();
				sb.AppendFormat("Mana (MP): {0}/{1}", Mana, MaxMana);
				sb.AppendLine();
				sb.AppendFormat("Strength (STR): {0}", Strength);
				sb.AppendLine();
				sb.AppendFormat("Intelligence (INT): {0}", Intelligence);
				sb.AppendLine();
				sb.AppendFormat("Physical Defense (PDF): {0}", PhysicalDefense);
				sb.AppendLine();
				sb.AppendFormat("Magical Defense (MDF): {0}", MagicalDefense);
				sb.AppendLine();
				sb.AppendFormat("Weapon: {0}", EquippedWeapon == null ? "None" : EquippedWeapon.DisplayName);
				sb.AppendLine();
				if (KnownSpells != null && KnownSpells.Any())
				{
					sb.Append("Known Spells: ");
					KnownSpells.ForEach(x => sb.AppendFormat("{0} ({1} mana), ", x.DisplayName, x.Cost));
					sb.Remove(sb.Length - 2, 2);
				}

				return sb.ToString();				
			}
		}

		private int health;
		public int Health
		{
			get { return health; }
			set { health = Math.Max(0, Math.Min(MaxHealth, value)); }
		}

		public abstract int MaxHealth { get; }

		private int mana;
		public int Mana
		{
			get { return mana; }
			set { mana = Math.Max(0, Math.Min(MaxMana, value)); }
		}

		public abstract int MaxMana { get; }		

		private int level = 1;
		public int Level
		{
			get { return level; }
			set 
			{
				int oldLevel = level;
				level = Math.Max(1, value);

				if (level > oldLevel)
					LeveledUp();
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
				}
			}
		}
		public long NeededExperience { get { return Level * Level; } }

		public int Strength { get; set; }
		public int Intelligence { get; set; }
		public int PhysicalDefense { get; set; }
		public int MagicalDefense { get; set; }

		public List<GameSpell> KnownSpells { get; set; }
		public List<StatusEffect> CurrentStatus { get; set; }

		public Weapon EquippedWeapon { get; set; }
		#endregion

		#region Events
		protected event Action LeveledUp;
		protected event Action<ReceivingDamageEventArgs> ReceivingDamage;
		#endregion

		protected Person()
		{
			Health = MaxHealth;
			Mana = MaxMana;

			KnownSpells = new List<GameSpell>();
			CurrentStatus = new List<StatusEffect>();

			LeveledUp += LeveledUpEventHandler;
			ReceivingDamage += ReceivingDamageEventHandler;
		}

		public virtual void Update() 
		{ 
			// Copy the status effect list so the original can be modified (when effects wear off, they remove themselves)
			List<StatusEffect> copyStatus = new List<StatusEffect>(CurrentStatus);
			copyStatus.ForEach(x => x.Update());

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

		public virtual void Talk()
		{
			Console.WriteLine("{0} doesn't answer", DisplayName);
		}

		public GameSpell GetSpellWithName(string name)
		{
			IList<GameSpell> found = (from item in KnownSpells
									  where item.CodeNames.Contains(name)
									  select item)
									 .ToList();

			if (found.Count == 0)
			{
				Console.WriteLine("There is no '{0}'", name);
				return null;
			}
			else if (found.Count > 1)
			{
				Console.WriteLine("There are multiple '{0}'. Please specify.", name);
				return GameSystem.Choice<GameSpell>(found, found.Select(x => x.DisplayName).ToList());
			}
			else
			{
				return found[0];
			}
		}

		public int ReceiveDamage(int baseDamage, DamageType type, object source)
		{
			ReceivingDamageEventArgs eventArgs = new ReceivingDamageEventArgs(baseDamage, type, source);			

			ReceivingDamage(eventArgs);

			Console.WriteLine("{0} was damaged for {1} HP", DisplayName, eventArgs.ActualDamage);
			Health -= eventArgs.ActualDamage;
			return eventArgs.ActualDamage;
		}
		
		#region Event Handlers
		void LeveledUpEventHandler()
		{
			Console.WriteLine("{0} grew to level {1}!", DisplayName, Level);
		}

		void ReceivingDamageEventHandler(ReceivingDamageEventArgs eventArgs)
		{
			switch (eventArgs.Type)
			{
				case DamageType.Physical:
					eventArgs.ActualDamage = Math.Max(0, eventArgs.ActualDamage - PhysicalDefense);
					break;
				case DamageType.Magical:
					eventArgs.ActualDamage = Math.Max(0, eventArgs.ActualDamage - MagicalDefense);
					break;
			}
		}
		#endregion

		public class ReceivingDamageEventArgs : EventArgs
		{
			public readonly int BaseDamage;
			public readonly DamageType Type;
			public readonly object Source;

			private int actualDamage;
			public int ActualDamage
			{
				get { return actualDamage; }
				set { actualDamage = Math.Max(0, value); }
			}

			public ReceivingDamageEventArgs(int baseDamage, DamageType type, object source)
			{
				this.BaseDamage = baseDamage;
				this.Type = type;
				this.Source = source;
				ActualDamage = BaseDamage;
			}
		}
	}
}
