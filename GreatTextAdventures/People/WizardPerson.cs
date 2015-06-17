using GreatTextAdventures.Spells;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GreatTextAdventures.People
{
	public class WizardPerson : Person
	{
		const int ExperiencePerLevel = 4;
		const int ExperienceDeltaMultiplier = 15;

		const int HealthMinimum = 20;
		const int HealthPerLevel = 10;

		const int ManaMinimum = 100;
		const int ManaPerLevel = 20;

		const int CriticalHealthDivider = 4;
		const int CriticalManaDivider = 5;

		const int IntMinPerLevel = 1;
		const int IntMaxPerLevel = 2;

		bool detected = false;

		public override string DisplayName
		{
			get { return string.Format("Wizard (Lv {0})", Level); }
		}
		public override IEnumerable<string> CodeNames
		{
			get { yield return "wizard"; }
		}

		public override int MaxHealth
		{
			get { return HealthMinimum + HealthPerLevel * (Level - 1); }
		}
		public override int MaxMana
		{
			get { return ManaMinimum + ManaPerLevel * (Level - 1); }
		}

		public WizardPerson(int level) : base()
		{
			this.Level = level;

			Initialize();

			KnownSpells.Add(new HealSpell(Level));
			KnownSpells.Add(new FireballSpell(GameSystem.RNG.Next(Level - 2, Level + 1)));
			KnownSpells.Add(new ManaHealSpell(Level));

			Strength = 1;
			Intelligence = GameSystem.RNG.Next(IntMinPerLevel * Level, IntMaxPerLevel * Level);
		}

		public override void Update()
		{
			base.Update();

			if (Health <= 0)
			{
				int delta = this.Level - GameSystem.Player.Level;
				// Math.Max is used to ensure the 'delta bonus' is always positive
				int exp = ExperiencePerLevel * Level + Math.Max(0, delta * ExperienceDeltaMultiplier);

				Console.WriteLine("{0} dropped {1} experience", DisplayName, exp);
				GameSystem.Player.Experience += exp;

				return;
			}

			if (!detected)
			{
				detected = true;
				Console.WriteLine("{0} has detected {1}!", DisplayName, GameSystem.Player.DisplayName);
				return;
			}

			if (this.Health <= this.MaxHealth / CriticalHealthDivider)
			{
				HealSpell heal = (HealSpell)KnownSpells.First(x => x is HealSpell);

				if (Mana >= heal.Cost)
				{
					heal.Cast(this, this);
					return;
				}
			}

			if (this.Mana <= this.MaxMana / CriticalManaDivider)
			{
				ManaHealSpell refresh = (ManaHealSpell)KnownSpells.First(x => x is ManaHealSpell);

				if (Health > refresh.HealthCost)
				{
					refresh.Cast(this, this);
					return;
				}
			}

			FireballSpell fire = (FireballSpell)KnownSpells.First(x => x is FireballSpell);

			if (Mana >= fire.Cost)
			{
				fire.Cast(this, GameSystem.Player);
				return;
			}

			Console.WriteLine("{0} punched {1}", DisplayName, GameSystem.Player.DisplayName);
			GameSystem.Player.ReceiveDamage(1, DamageType.Physical);
		}

		public override void Talk()
		{
			Console.WriteLine("\"My power is far superior to yours!\"");
		}

	}
}
