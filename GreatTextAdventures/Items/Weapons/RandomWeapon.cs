using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GreatTextAdventures.Items.Weapons
{
	public class RandomWeapon : Weapon
	{	

		public RandomWeapon(string name, string nameModifier, int attack, int attackModifier)
		{
			this.baseName = name;
			this.nameModifier = nameModifier;
			this.baseAttack = attack;
			this.attackModifier = attackModifier;
		}

		public static RandomWeapon Random(int level)
		{
			JObject weapons = JObject.Parse(File.ReadAllText(@"Items\Weapons\Weapons.json"));
			JArray names = (JArray)weapons["Names"];
			JArray modifiers = (JArray)weapons["Modifiers"];

			JArray chosenModifier = (JArray)modifiers.OrderBy(x => GameSystem.RNG.Next()).First();
			string chosenName = (string)names.OrderBy(x => GameSystem.RNG.Next()).First();

			return new RandomWeapon(chosenName, (string)chosenModifier[0], GameSystem.RNG.Next(level * 5, level * 10), (int)chosenModifier[1]);
		}
	}
}
