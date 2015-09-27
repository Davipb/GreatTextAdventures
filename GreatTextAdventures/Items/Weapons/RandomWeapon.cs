using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;

namespace GreatTextAdventures.Items.Weapons
{
	public class RandomWeapon : Weapon
	{	
		public static JObject AllWeapons { get; private set; }

		protected RandomWeapon(string name, string nameMod, int attack, int attackMod)
		{
			baseName = name;
			nameModifier = nameMod;
			baseAttack = attack;
			attackModifier = attackMod;
		}

		public static RandomWeapon Generate(int level)
		{
			if (AllWeapons == null)
				AllWeapons = JObject.Parse(File.ReadAllText(@"Items\Weapons\Weapons.json"));

			var names = (JArray)AllWeapons["Names"];
			var modifiers = (JArray)AllWeapons["Modifiers"];

			var chosenModifier = (JArray)modifiers.OrderBy(x => GameSystem.RNG.Next()).First();
			var chosenName = (string)names.OrderBy(x => GameSystem.RNG.Next()).First();

			return new RandomWeapon(chosenName, (string)chosenModifier[0], GameSystem.RNG.Next(level * 5, level * 10), (int)chosenModifier[1]);
		}
	}
}
