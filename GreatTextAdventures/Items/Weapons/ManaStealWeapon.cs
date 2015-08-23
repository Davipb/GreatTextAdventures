using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreatTextAdventures.People;

namespace GreatTextAdventures.Items.Weapons
{
	public class ManaStealWeapon : Weapon
	{
		const int ManaStealPerLevel = 5;
		protected int level;

		public override string Description
		{
			get
			{
				var sb = new StringBuilder();
				sb.AppendLine($"Attack: {Attack}");
				sb.AppendLine($"Strength Bonus: +{StrBonus * 100f}%/STR");
				sb.AppendLine($"Steals {ManaStealPerLevel * level} from the enemy on hit");
				return sb.ToString();			
			}
		}

		public ManaStealWeapon(int level, int baseAttack) 
		{
			this.level = level;
			this.baseAttack = baseAttack;
			this.baseName = "Blade";
			this.nameModifier = "Mana-Stealing";

			Hit += StealMana;
		}

		void StealMana(Person user, Person target, int damage)
		{			
			int before = target.Mana;
			target.Mana -= ManaStealPerLevel * level;

			if (before > target.Mana)
			{
				GameSystem.WriteLine($"{target.DisplayName} lost {before - target.Mana} mana");

				user.Mana += before - target.Mana;
				GameSystem.WriteLine($"{user.DisplayName} recovered {before - target.Mana} mana");
			}

		}
	}
}
