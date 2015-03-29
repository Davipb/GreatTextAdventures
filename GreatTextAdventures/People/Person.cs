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
				sb.AppendFormat("Health: {0}/{1}", Health, MaxHealth);
				sb.AppendLine();
				sb.AppendFormat("Mana: {0}/{1}", Mana, MaxMana);
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

		public List<GameSpell> KnownSpells { get; set; }
		public List<StatusEffect> CurrentStatus { get; set; }

		public Weapon EquippedWeapon { get; set; }

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

		protected void Initialize()
		{
			Health = MaxHealth;
			Mana = MaxMana;

			KnownSpells = new List<GameSpell>();
			CurrentStatus = new List<StatusEffect>();
		}
	}
}
