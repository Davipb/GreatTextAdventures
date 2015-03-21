using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace GreatTextAdventures.Items
{
	public class Weapon : ILookable
	{
		public string DisplayName { get { return string.Format("{0} {1} (Atk {2})", nameModifier, name, Attack); } }
		public string Description { get { throw new NotImplementedException(); } }
		public IEnumerable<string> CodeNames { get { yield return name.ToLowerInvariant(); } }

		public int Attack { get { return Math.Max(1, attack + attackModifier); } }

		protected string name;
		protected string nameModifier;
		protected int attack;
		protected int attackModifier;

		public Weapon(string name, string nameModifier, int attack, int attackModifier)
		{
			this.name = name;
			this.nameModifier = nameModifier;
			this.attack = attack;
			this.attackModifier = attackModifier;
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
