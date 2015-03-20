using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;
using System.Linq;

namespace GreatTextAdventures.Items
{
	public class Weapon : Item
	{
		public int Attack { get; set; }

		public static Weapon Random()
		{
			Weapon result = new Weapon();
			result.Attack = GameSystem.RNG.Next(1, 101);
			StringBuilder weaponName = new StringBuilder();

			JObject weapons = JObject.Parse(File.ReadAllText(@"Items\Weapons.json"));
			JArray names = (JArray)weapons["Names"];
			JArray modifiers = (JArray)weapons["Modifiers"];

			JArray chosenModifier = (JArray)modifiers.OrderBy(x => GameSystem.RNG.Next()).First();
			weaponName.Append(chosenModifier[0]);
			result.Attack += (int)chosenModifier[1];

			weaponName.Append(" ");

			string chosenName = (string)names.OrderBy(x => GameSystem.RNG.Next()).First();
			weaponName.Append(chosenName);
			result.CodeNames = new[] { chosenName.ToLowerInvariant() };
			result.DisplayName = weaponName.ToString();

			return result;
		}
	}
}
