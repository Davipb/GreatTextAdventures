﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GreatTextAdventures.People;
using System.Threading.Tasks;

namespace GreatTextAdventures.Items.Weapons
{
	public abstract class Weapon : ILookable
	{
		protected const float StrBonus = 0.01f;

		public virtual string DisplayName => $"{nameModifier} {baseName} (Atk {Attack})";
		public virtual string Description
		{
			get
			{
				var sb = new StringBuilder();
				sb.AppendLine($"Base Attack: {baseAttack}");
				sb.AppendLine($"Attack Modifier: {(attackModifier == 0 ? "" : attackModifier > 0 ? "+" : "-")}{Math.Abs(attackModifier)}");
				sb.Append($"Strength Bonus: +{StrBonus * 100f}%/STR");
				return sb.ToString();
			}
		}
		public virtual IEnumerable<string> CodeNames
		{
			get
			{
				yield return baseName.ToLowerInvariant();
				yield return (nameModifier + " " + baseName).ToLowerInvariant();
			}
		}
		public bool CanTake => true;

		public virtual int Attack => Math.Max(1, baseAttack + attackModifier);

		public virtual int Damage(Person user) => Attack + (int)Math.Ceiling(StrBonus * user.Strength * Attack);

		public event Action<Person, Person, int> Hit;

		public void OnHit(Person user, Person target, int damage) => Hit?.Invoke(user, target, damage);

		protected string baseName;
		protected string nameModifier;
		protected int baseAttack;
		protected int attackModifier;
	}
}
