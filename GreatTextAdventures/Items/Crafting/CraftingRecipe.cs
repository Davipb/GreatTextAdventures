using GreatTextAdventures.Items.Weapons;
using System;
using System.Collections.Generic;
using System.Text;

namespace GreatTextAdventures.Items.Crafting
{
	public class CraftingRecipe : ILookable
	{
		public string DisplayName => $"Crafting Recipe ({Result.DisplayName})";
		public IEnumerable<string> CodeNames
		{
			get
			{
				yield return "crafting recipe";
				yield return "recipe";
				foreach (var codename in Result.CodeNames)
					yield return codename;
			}
		}
		public string Description
		{
			get
			{
				var sb = new StringBuilder();
				sb.AppendLine("Ingredients:");

				foreach (var ingredient in Ingredients)
					sb.AppendLine($"{ingredient.Value}x {CraftingMaterial.AllMaterials[ingredient.Key]["DisplayName"]}");

				sb.AppendLine();
				sb.AppendLine("Result:");
				sb.Append(Result.DisplayName);
				sb.Append(Result.Description);

				return sb.ToString();
			}
		}
		public bool CanTake => true;

		public IDictionary<string, int> Ingredients { get; }
		public ILookable Result { get; }

		public CraftingRecipe(IDictionary<string, int> ingredients, ILookable result)
		{
			Ingredients = ingredients;
			Result = result;
		}

		public static CraftingRecipe Generate(int level)
		{
			switch (GameSystem.RNG.Next(0, 3))
			{
				case 0:
					return GenerateWithSpell(level);
				case 1:
					return GenerateWithRandomWeapon(level);
				case 2:
					return GenerateWithSpecialWeapon(level);
			}

			throw new Exception("Black Magic");
		}

		public static CraftingRecipe GenerateWithSpell(int level)
		{
			var ingredients = new Dictionary<string, int>();
			ingredients.Add("Parchment", 1);
			ingredients.Add("MagicRune", GameSystem.RNG.Next(1, level + 1));

			return new CraftingRecipe(ingredients, SpellTome.Random(level));
		}

		public static CraftingRecipe GenerateWithRandomWeapon(int level)
		{
			var ingredients = new Dictionary<string, int>();
			ingredients.Add("IronIngot", GameSystem.RNG.Next(1, level));

			return new CraftingRecipe(ingredients, RandomWeapon.Generate(level));
		}

		public static CraftingRecipe GenerateWithSpecialWeapon(int level)
		{
			var ingredients = new Dictionary<string, int>();
			ingredients.Add("IronIngot", GameSystem.RNG.Next(1, level));

			int baseAttack = GameSystem.RNG.Next(level * 5, level * 10);
			ILookable returnObject = null;

			switch (GameSystem.RNG.Next(0, 2))
			{
				case 0:
					ingredients.Add("MagicRune", GameSystem.RNG.Next(1, level + 1));
					returnObject = new ManaStealWeapon(level, baseAttack);
					break;
				case 1:
					ingredients.Add("PoisonVial", GameSystem.RNG.Next(1, level + 1));
					returnObject = new PoisonWeapon(level, baseAttack);
					break;
			}

			return new CraftingRecipe(ingredients, returnObject);
		}
	}
}
