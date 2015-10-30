using GreatTextAdventures.Items.Weapons;
using GreatTextAdventures.Spells;
using GreatTextAdventures.StatusEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GreatTextAdventures.People
{
	public abstract class Person : ILookable, IUpdatable
	{
		#region Properties
		public abstract string DisplayName { get; }
		public abstract IEnumerable<string> CodeNames { get; }
		public bool CanTake => false;

		public string Description
		{
			get
			{
				var sb = new StringBuilder();

				sb.AppendLine($"Level: {level}");
				sb.AppendLine($"Experience: {Experience}/{NeededExperience}");
				sb.AppendLine($"Health (HP): {Health}/{MaxHealth}");
				sb.AppendLine($"Mana (MP): {Mana}/{MaxMana}");
				sb.AppendLine($"Strength (STR): {Strength}");
				sb.AppendLine($"Intelligence (INT): {Intelligence}");
				sb.AppendLine($"Physical Defense (PDF): {PhysicalDefense}");
				sb.AppendLine($"Magical Defense (MDF): {MagicalDefense}");
				sb.AppendLine($"Weapon: {EquippedWeapon?.DisplayName ?? "None"}");
				sb.Append(GameSystem.Enumerate(KnownSpells.Select(x => x.DisplayName), "Known Spells:", "Known Spell:", "No spells known", "and"));

				return sb.ToString();
			}
		}

		#region Stats
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
					LeveledUp?.Invoke();
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
		public long NeededExperience => Level * Level;

		public int Strength { get; set; }
		public int Intelligence { get; set; }
		public int PhysicalDefense { get; set; }
		public int MagicalDefense { get; set; }
		#endregion

		public List<GameSpell> KnownSpells { get; } = new List<GameSpell>();
		public List<StatusEffect> CurrentStatus { get; } = new List<StatusEffect>();

		public List<ILookable> Inventory { get; } = new List<ILookable>();
		public Weapon EquippedWeapon { get; set; }


		#endregion

		#region Events
		protected event Action LeveledUp;
		protected event Action<ReceivingDamageEventArgs> ReceivingDamage;
		#endregion

		protected Person(int level)
		{
			Health = MaxHealth;
			Mana = MaxMana;
			Level = level;

			LeveledUp += LeveledUpEventHandler;
			ReceivingDamage += ReceivingDamageEventHandler;
		}

		public virtual void Update()
		{
			// Copy the status effect list so the original can be modified (when effects wear off, they remove themselves)
			var copyStatus = new List<StatusEffect>(CurrentStatus);
			copyStatus.ForEach(x => x.Update());

			// ToList() used to prevent LINQ's lazy evaluation, since the base collection may be modified
			var inv = Inventory.OfType<IUpdatable>().ToList();
			foreach (var item in inv) item.Update();

			if (Health <= 0)
			{
				GameSystem.WriteLine($"{DisplayName} died");

				if (EquippedWeapon != null)
				{
					GameSystem.WriteLine($"{DisplayName} dropped {EquippedWeapon.DisplayName}");

					GameSystem.CurrentMap.CurrentRoom.Members.Add(EquippedWeapon);
					EquippedWeapon = null;
				}

				foreach (ILookable item in Inventory)
				{
					GameSystem.WriteLine($"{DisplayName} dropped {item.DisplayName}");
					GameSystem.CurrentMap.CurrentRoom.Members.Add(item);
				}

				Inventory.Clear();

				GameSystem.CurrentMap.CurrentRoom.Members.Remove(this);
			}
		}

		public virtual void Talk()
		{
			GameSystem.WriteLine($"{DisplayName} doesn't answer");
		}

		public GameSpell GetSpellWithName(string name)
		{
			IList<GameSpell> found = (from item in KnownSpells
									  where item.CodeNames.Contains(name)
									  select item)
									 .ToList();

			if (found.Count == 0)
			{
				GameSystem.WriteLine($"There is no '{name}'");
				return null;
			}
			if (found.Count > 1)
			{
				GameSystem.WriteLine($"There are multiple '{name}'. Please specify.");
				return GameSystem.Choice(found, found.Select(x => x.DisplayName).ToList());
			}
			return found[0];
		}

		public int ReceiveDamage(int baseDamage, DamageType type, object source)
		{
			var eventArgs = new ReceivingDamageEventArgs(baseDamage, type, source);

			ReceivingDamage?.Invoke(eventArgs);

			var damager = source as ILookable;

			if (damager != null)
				GameSystem.WriteLine($"{DisplayName} was damaged for {eventArgs.ActualDamage} HP by {damager.DisplayName}");
			else
				GameSystem.WriteLine($"{DisplayName} was damaged for {eventArgs.ActualDamage} HP");

			Health -= eventArgs.ActualDamage;
			return eventArgs.ActualDamage;
		}

		public int Attack(Person target)
		{
			GameSystem.WriteLine($"{DisplayName} attacked {target.DisplayName} with {EquippedWeapon?.DisplayName ?? "their fists"}");

			int damage = target.ReceiveDamage(
				EquippedWeapon?.Damage(this) ?? 1,
				DamageType.Physical,
				this);

			EquippedWeapon.OnHit(this, target, damage);

			return damage;
		}

		#region Event Handlers
		void LeveledUpEventHandler()
		{
			GameSystem.WriteLine($"{DisplayName} grew to level {Level}!");
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
				BaseDamage = baseDamage;
				Type = type;
				Source = source;
				ActualDamage = BaseDamage;
			}
		}
	}
}
