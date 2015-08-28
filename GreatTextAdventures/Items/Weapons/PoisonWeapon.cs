using System.Text;
using GreatTextAdventures.People;
using GreatTextAdventures.StatusEffects;

namespace GreatTextAdventures.Items.Weapons
{
	public class PoisonWeapon : Weapon
	{
		const int PoisonDurationPerLevel = 2;
		protected int poisonChance;
		protected int level;

		public override string Description
		{
			get
			{
				var sb = new StringBuilder();
				sb.AppendLine($"Attack: {Attack}");
				sb.AppendLine($"Strength Bonus: +{StrBonus * 100f}%/STR");
				sb.Append($"{poisonChance}% chance to poison the enemy for {PoisonDurationPerLevel * level} turns on hit");
				return sb.ToString();
			}
		}

		public PoisonWeapon(int level, int baseAttack)
		{
			this.level = level;
			this.baseAttack = baseAttack;
			this.baseName = "Blade";
			this.nameModifier = "Poisoned";
			this.poisonChance = GameSystem.RNG.Next(25, 76);

			Hit += Poison;
		}

		void Poison(Person user, Person target, int damage)
		{
			if (GameSystem.RNG.Next(0, 100) < poisonChance)
			{
				GameSystem.WriteLine($"{user.DisplayName}'s {DisplayName} poisoned {target.DisplayName}!");
				target.CurrentStatus.Add(new PoisonEffect(target, PoisonDurationPerLevel * level));
			}
		}
	}
}
