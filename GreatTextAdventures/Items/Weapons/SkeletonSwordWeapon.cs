using GreatTextAdventures.People;
using System.Collections.Generic;

namespace GreatTextAdventures.Items.Weapons
{
	public class SkeletonSwordWeapon : Weapon
	{
		const int MaxPlayerUses = 2;

		int PlayerUses = 0;

		public override string Description
			=> "Maintained alive by the skeleton power, it deteriorates when held by anyone else.\n" + base.Description;

		public override IEnumerable<string> CodeNames
		{
			get
			{
				yield return "sword";
				yield return "skeleton sword";
				yield return nameModifier.ToLowerInvariant() + " skeleton sword";
			}
		}

		public SkeletonSwordWeapon(int level)
		{
			baseAttack = level * 10;
			baseName = "Skeleton Sword";

			switch (GameSystem.RNG.Next(0, 10))
			{
				case 0:
					nameModifier = "Spooky";
					attackModifier = (int)(baseAttack * .5);
					break;
				case 1:
					nameModifier = "Scary";
					attackModifier = (int)(baseAttack * .25);
					break;
				case 2:
					nameModifier = "Lame";
					attackModifier = (int)(baseAttack * -.25);
					break;
				case 3:
					nameModifier = "Damaged";
					attackModifier = (int)(baseAttack * -.5);
					break;
				default:
					nameModifier = "Regular";
					attackModifier = 0;
					break;
			}

			Hit += DamageWeapon;
		}

		void DamageWeapon(Person user, Person target, int damage)
		{
			var player = user as PlayerPerson;

			if (player == null || player != GameSystem.Player || PlayerUses >= MaxPlayerUses)
				return;

			PlayerUses++;
			if (PlayerUses >= MaxPlayerUses)
			{
				GameSystem.WriteLine($"{DisplayName} breaks in your hand");
				nameModifier = "Broken";
				attackModifier = -baseAttack;
			}
			else
			{
				GameSystem.WriteLine($"{DisplayName} was damaged. Uses before breaking: {MaxPlayerUses - PlayerUses}");
			}
		}
	}
}
