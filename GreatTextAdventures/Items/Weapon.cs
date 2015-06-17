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

		public string DisplayName { get { return string.Format("{0} {1} (Atk {2})", nameModifier, baseName, Attack); } }
		public string Description
		{
			get
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendFormat("{0} {1}", nameModifier, baseName);
				sb.AppendLine();
				sb.AppendFormat("Base Attack: {0}", baseAttack);
				sb.AppendLine();
				sb.AppendFormat("Attack Modifier: {0}{1}", Math.Sign(attackModifier) == 0? "" : Math.Sign(attackModifier) > 0? "+" : "-", Math.Abs(attackModifier));
				sb.AppendLine();
				sb.AppendFormat("Strength Bonus: +{0}%/STR", StrBonus * 100f);
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

		public int Attack { get { return Math.Max(1, baseAttack + attackModifier); } }

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

		public int Damage(People.Person user)
		{
			return Attack + (int)Math.Ceiling(StrBonus * user.Strength * Attack);
		}

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
