using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GreatTextAdventures.Items
{
	public class Weapon : ILookable
	{
		const float StrBonus = 0.01f;

		public string DisplayName => $"{nameModifier} {baseName} (Atk {Attack})";
		public string Description
		{
			get
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendLine($"{nameModifier} {baseName}");
				sb.AppendLine($"Base Attack: {baseAttack}");
				sb.AppendLine($"Attack Modifier: {(attackModifier == 0? "" : attackModifier > 0? "+" : "-")}{Math.Abs(attackModifier)}");
				sb.AppendLine($"Strength Bonus: +{StrBonus * 100f}%/STR");
				return sb.ToString();

			}
		}
		public IEnumerable<string> CodeNames 
		{ 
			get 
			{ 
				yield return baseName.ToLowerInvariant();
				yield return (nameModifier + " " + baseName).ToLowerInvariant();
			} 
		}
		public bool CanTake => true;

		public int Attack => Math.Max(1, baseAttack + attackModifier);

		protected string baseName;
		protected string nameModifier;
		protected int baseAttack;
		protected int attackModifier;

		public Weapon(string name, string nameModifier, int attack, int attackModifier)
		{
			this.baseName = name;
			this.nameModifier = nameModifier;
			this.baseAttack = attack;
			this.attackModifier = attackModifier;
		}

		public int Damage(People.Person user) => Attack + (int)Math.Ceiling(StrBonus* user.Strength * Attack);

		public static Weapon Random(int level)
		{
			JObject weapons = JObject.Parse(File.ReadAllText(@"Items\Weapons.json"));
			JArray names = (JArray)weapons["Names"];
			JArray modifiers = (JArray)weapons["Modifiers"];

			JArray chosenModifier = (JArray)modifiers.OrderBy(x => GameSystem.RNG.Next()).First();
			string chosenName = (string)names.OrderBy(x => GameSystem.RNG.Next()).First();

			return new Weapon(chosenName, (string)chosenModifier[0], GameSystem.RNG.Next(level * 5, level * 10), (int)chosenModifier[1]);
		}

		public void Update() { /* ¯\_(ツ)_/¯ */ }
	}
}
